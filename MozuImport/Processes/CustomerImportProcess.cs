using System;
using System.Collections.Generic;
using System.Linq;
using Mozu.Api.Contracts.Core;
using Mozu.Api.Contracts.Customer;
using Mozu.Api.Resources.Commerce.Customer;
using Mozu.Api.Resources.Commerce.Customer.Accounts;
using MozuImport.Excel;
using MozuImport.Models;

namespace MozuImport.Processes
{
    public class CustomerImportProcess : AbstractWorkerProcess
    {
        private IDictionary<string, CustomerAccount> _existingAccounts = null;

        public override void Run(IProcessContext context)
        {
            Context = context;
            var customerImportFile = context.Parameters["CustomerImportFile"];

            ReportProgress("Start CustomerImport at " + DateTime.Now.ToShortTimeString());

            try
            {
                // Load existing accounts
                LoadExistingAccounts();

                var customers = ReadCustomerImportFile(customerImportFile);
                MaxCount = customers.Count();

                foreach (var customer in customers)
                {
                    ProcessedCount = ProcessedCount + 1;

                    if (IsCancellationPending)
                    {
                        ReportProgress("User cancelled process.");
                        break; //out
                    }

                    try
                    {
                        if (customer.Delete)
                        {
                            ReportProgress(string.Format("Deleting account: {0}, UserName: '{1}'",
                                customer.Account.Id,
                                customer.Account.UserName));

                            DeleteCustomer(customer);
                        }
                        else
                        {
                            ReportProgress(string.Format("Create or Update account: {0}, UserName: '{1}'",
                                customer.Account.Id,
                                customer.Account.UserName));

                            CreateCustomer(customer);
                        }
                    }
                    catch (Exception e)
                    {
                        string customerData;
                        // Format some customer data so we know which one failed
                        if (customer.Account != null)
                        {
                            customerData = string.Format("Exception for {0}: {1}",
                                customer.Account.Id,
                                customer.Account.UserName);
                        }
                        else
                        {
                            customerData = "No customer data";
                        }

                        ReportProgress(customerData);
                        ReportProgress(e);

                        // on to the next one
                    }
                }
            }
            catch (Exception e)
            {
                ReportProgress(e);
            }

            ReportProgress(string.Format("Processed {0} Items", ProcessedCount));
            ReportProgress("End Customer Import " + DateTime.Now.ToShortTimeString());

        }

        private IDictionary<string, CustomerAccount> LoadExistingAccounts()
        {
            _existingAccounts = new Dictionary<string, CustomerAccount>();

            var customerAccountResource = new CustomerAccountResource(Context.ApiContext);
            var customerAccounts = customerAccountResource.GetAccounts();
            foreach (CustomerAccount account in customerAccounts.Items)
            {
                // Add to accountid lookup dictionary
                //_existingAccounts.Add(account.Id.ToString(), account);
                if (!string.IsNullOrEmpty(account.UserName))
                {
                    _existingAccounts.Add(account.UserName, account);
                }
            }
            return _existingAccounts;
        }

        private IEnumerable<CustomerModel> ReadCustomerImportFile(string customerImportFile)
        {
            var customersToImport = new List<CustomerModel>();

            var customerAccounts = new Worksheet();
            customerAccounts.Load(customerImportFile, "CustomerAccounts", "AccountID");

            var customerContacts = new Worksheet();
            customerContacts.Load(customerImportFile, "CustomerContacts", "AccountId");

            RowData row = null;
            while ((row = customerAccounts.NextRow()) != null)
            {
                var customer = new CustomerModel();
                customer.Account = new CustomerAccount();
                customer.Account.Id = row.Cell("AccountID").ToInt32(0);
                customer.Account.UserName = row.Cell("UserName");
                customer.Account.EmailAddress = row.Cell("Email");
                customer.Account.FirstName = row.Cell("FirstName");
                customer.Account.LastName = row.Cell("LastNameOrSurname");
                customer.Account.CompanyOrOrganization = row.Cell("CompanyOrOrganization");
                customer.Account.TaxExempt = row.Cell("TaxExempt").ToBoolean(false);
                customer.Account.TaxId = row.Cell("TaxId");
                customer.Account.AcceptsMarketing = row.Cell("AcceptsMarketing").ToBoolean(false);
                customer.Account.LocaleCode = row.Cell("LocaleCode");
                //customer.Account.UserRole = row.Cell("UserRole");
                //customer.Account.IsActive = row.Cell("IsActive").ToBoolean(true);
                customer.Account.IsAnonymous = row.Cell("IsAnonymous").ToBoolean(false);
                //customer.Account.TotalOrderAmount = row.Cell("Total Order Amount").ToDecimal(new decimal(0.00));
                //customer.Account.TotalOrders = row.Cell("Total Orders").ToInt32(0);
                //customer.Account.LastOrderDate = DateTime.MinValue;
                //customer.Account.TotalContacts = 0;
                customer.Password = row.Cell("Password");

                // Possible Delete
                if (customerAccounts.HasColumn("Delete"))
                {
                    customer.Delete = row.Cell("Delete").ToBoolean(false);
                }

                customersToImport.Add(customer);

                IList<RowData> contactRows = customerContacts.GetRows(customer.Account.Id.ToString());
                foreach (var contactRow in contactRows)
                {
                    // Populate the CustomerContact
                    var contact = new CustomerContact();
                    contact.AccountId = customer.Account.Id;
                    contact.Types = new List<ContactType>() { new ContactType() { IsPrimary = true, Name = "Billing" } };
                    contact.FirstName = contactRow.Cell("FirstName");
                    contact.LastNameOrSurname = contactRow.Cell("LastNameOrSurname");
                    contact.MiddleNameOrInitial = contactRow.Cell("MiddleNameorInitial");
                    contact.CompanyOrOrganization = contactRow.Cell("CompanyOrOrganization");
                    contact.Email = contactRow.Cell("Email");
                    contact.FaxNumber = contactRow.Cell("FaxNumber");
                    contact.PhoneNumbers = new Phone()
                    {
                        Home = contactRow.Cell("HomePhone"),
                        Mobile = contactRow.Cell("MobilePhone"),
                        Work = contactRow.Cell("WorkPhone")
                    };
                    contact.Address = new Address()
                    {
                        Address1 = contactRow.Cell("Address1"),
                        Address2 = contactRow.Cell("Address2"),
                        Address3 = contactRow.Cell("Address3"),
                        Address4 = contactRow.Cell("Address4"),
                        AddressType = contactRow.Cell("AddressType"),
                        CityOrTown = contactRow.Cell("CityOrTown"),
                        StateOrProvince = contactRow.Cell("StateOrProvince"),
                        CountryCode = contactRow.Cell("PostalOrZipCode"),
                        PostalOrZipCode = contactRow.Cell("CountryCode"),
                        IsValidated = true
                    };

                    customer.Contacts.Add(contact);
                }
            }

            return customersToImport;
        }

        private void DeleteCustomer(CustomerModel customer)
        {
            if (customer.Delete == false)
                throw new Exception("Will not delete account because it is not marked for deletion");

            // Delete the customer Account
            var customerAccountResource = new CustomerAccountResource(Context.ApiContext);
            customerAccountResource.DeleteAccount(customer.Account.Id);
        }

        private void CreateCustomer(CustomerModel customer)
        {
            // Create or Update the customerAccount
            var customerAccountResource = new CustomerAccountResource(Context.ApiContext);

            var existingAccount = ExistingAccount(customer.Account.UserName);
            if (existingAccount != null)
            {
                // Update existing account
                customer.Account.Id = existingAccount.Id;

                customer.Account = customerAccountResource.UpdateAccount(customer.Account, customer.Account.Id);
                ReportProgress("Account updated: " + customer.Account.Id);
            }
            else
            {
                // Add a new account
                customer.Account = customerAccountResource.AddAccount(customer.Account);
                ReportProgress("Account created: " + customer.Account.Id + " " + customer.Account.UserName);
            }

            // Set the password only if we have one
            if (!string.IsNullOrEmpty(customer.Password))
            {
                var loginInfo = new CustomerLoginInfo();
                loginInfo.EmailAddress = customer.Account.EmailAddress;
                loginInfo.IsImport = true;
                loginInfo.Username = customer.Account.UserName;
                loginInfo.Password = customer.Password;
                var customerAuth = customerAccountResource.AddLoginToExistingCustomer(loginInfo, customer.Account.Id);
                ReportProgress("Password Updated for : " + customer.Account.Id);
            }

            foreach (var contact in customer.Contacts)
            {
                // Update or Create the customer contact as required
                var customerContactResource = new CustomerContactResource(Context.ApiContext);

                // Find the existing contact of this type.
                CustomerContact existingContact = null;
                if (customer.Account.Contacts != null)
                {
                    foreach (var cc in customer.Account.Contacts)
                    {
                        foreach (var t in cc.Types)
                        {
                            if (t.Name == contact.Types[0].Name)
                            {
                                existingContact = cc;
                                break; // out
                            }
                            if (existingContact != null)
                            {
                                break; // out
                            }
                        }
                    }
                }

                if (existingContact != null)
                {
                    // update the existing contact
                    contact.Id = existingContact.Id;
                    customerContactResource.UpdateAccountContact(contact, customer.Account.Id, existingContact.Id);
                    ReportProgress("contact Updated: " + contact.Id + " " + contact.Email);
                }
                else
                {
                    // create a new contact
                    var newContact = customerContactResource.AddAccountContact(contact, customer.Account.Id);
                    ReportProgress("Contact Created Id: " + newContact.Id + " for " + newContact.Email);
                }
            }
        }

        private CustomerAccount ExistingAccount(string userName)
        {
            if (_existingAccounts.ContainsKey(userName))
                return _existingAccounts[userName];
            else
            {
                return null;
            }
        }
    }
}
