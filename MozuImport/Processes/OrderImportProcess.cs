using System;
using System.Collections.Generic;
using System.Linq;
using Mozu.Api.Contracts.CommerceRuntime.Commerce;
using Mozu.Api.Contracts.CommerceRuntime.Discounts;
using Mozu.Api.Contracts.CommerceRuntime.Fulfillment;
using Mozu.Api.Contracts.CommerceRuntime.Orders;
using Mozu.Api.Contracts.CommerceRuntime.Payments;
using Mozu.Api.Contracts.CommerceRuntime.Products;
using Mozu.Api.Contracts.Core;
using Mozu.Api.Contracts.Customer;
using Mozu.Api.Resources.Commerce;
using Mozu.Api.Resources.Commerce.Customer;
using MozuImport.Excel;
using MozuImport.Models;

namespace MozuImport.Processes
{


    public class OptionsObj
    {
        public string ProductCode { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }


    public class OrderHistoryImportProcess : AbstractWorkerProcess
    {
        // Default values
        private const string DefaultCurrencyCode = "usd";

        // members
        private IDictionary<string, CustomerAccount> _existingAccounts = null;

        public override void Run(IProcessContext context)
        {
            Context = context;
            var orderHistoryImportFile = context.Parameters["OrderHistoryImportFile"];
            var productOptionsFile = context.Parameters["ProductOptionsFile"];

            bool loadAllUsers = Convert.ToBoolean(context.Parameters["loadAllUsers"]);

            ReportProgress("Start OrderHistory Import at " + DateTime.Now.ToShortTimeString());

            try
            {
                // Read in all existing accounts
                LoadExistingAccounts(loadAllUsers);

                // Read in the OrderHistory import file
                var ordersToImport = ReadOrdersImportFile(orderHistoryImportFile, productOptionsFile);
                MaxCount = ordersToImport.Count();

                // Do the import
                foreach (var order in ordersToImport)
                {
                    ProcessedCount = ProcessedCount + 1;
                    if (IsCancellationPending)
                    {
                        ReportProgress("User cancelled process.");
                        break; //out
                    }

                    try
                    {

                       CreateOrder(order);
                    }
                    catch (Exception ex)
                    {
                        // on to the next one
                        ReportProgress("Error with order number " + order.Order.OrderNumber);
                        ReportProgress(ex);
                    }
                }
            }
            catch (Exception e)
            {
                ReportProgress(e);
            }

            ReportProgress(string.Format("Processed {0} Items", ProcessedCount));
            ReportProgress("End OrderHistory Import " + DateTime.Now.ToShortTimeString());

        }
        
        private IDictionary<string, CustomerAccount> LoadExistingAccounts(bool loadAllUsers)
        {
            _existingAccounts = new Dictionary<string, CustomerAccount>();

            var customerAccountResource = new CustomerAccountResource(Context.ApiContext);

                // change the start value by the page size until the start value is greater than the total

            var pageSize = 20;
            var pageCount = 1;
            ReportProgress("Begin GetAccounts call at " + DateTime.Now.ToShortTimeString());
            var customerAccounts = customerAccountResource.GetAccounts(null, pageSize);
            ReportProgress("Finish GetAccounts call at " + DateTime.Now.ToShortTimeString());

            var addedCount = 0;
            var skippedCount = 0;
             
            ReportProgress("Begin GetAccounts page " + pageCount + " call at " + DateTime.Now.ToShortTimeString());
            // skip paging for testing - comment out this for loop and its brackets - driven by checkbox on UI
            // skips loading all customer accounts into memory since it takes 3 hours on a production tenant with 39,000 users
            var upperLimit = 2;           
            if (loadAllUsers) {
                upperLimit = customerAccounts.TotalCount;
            }
            for (pageCount = 1; pageCount < upperLimit; pageCount = pageCount + customerAccounts.PageSize)
            {
                customerAccounts = customerAccountResource.GetAccounts(pageCount, pageSize);

                foreach (CustomerAccount account in customerAccounts.Items)
                {
                    // Add to accountid lookup dictionary
                    //_existingAccounts.Add(account.Id.ToString(), account);
                    string userName = account.UserName;
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = account.EmailAddress;
                    }

                    if (!string.IsNullOrEmpty(userName) & !_existingAccounts.ContainsKey(userName.ToLower()))
                    {
                        _existingAccounts.Add(userName.ToLower(), account);
                        addedCount++;
                    }
                    else
                    {
                        skippedCount++;
                        ReportProgress("skipping " + account.UserId + ":" + account.UserName + " - " + account.FirstName + " " + account.LastName + " with email address of " + account.EmailAddress);
                        
                    }
                }
            }
            ReportProgress("Finish GetAccounts page " + pageCount + " call at " + DateTime.Now.ToShortTimeString());

            return _existingAccounts;
        }

        private IEnumerable<OrderModel> ReadOrdersImportFile(string ordersImportFile, string productOptionsFile)
        {
            ReportProgress("Reading " + ordersImportFile);
            var ordersToImport = new List<OrderModel>();

            var orderSheet = new Worksheet();
            orderSheet.Load(ordersImportFile, "Orders", "Order Number");

            var lineItemsSheet = new Worksheet();
            lineItemsSheet.Load(ordersImportFile, "Order Line Items", "Order Number");


            // load all of product options into data structure since we access it twice for each row in the order list
            ReportProgress("Reading " + productOptionsFile);
            var optionSheet = new Worksheet();
            optionSheet.Load(productOptionsFile, "ProductOptions", "ProductCode");
            RowData variant = null;

            List<string> optionValues = new List<string>();
            Dictionary<string, OptionsObj> productVariantMapping = new Dictionary<string, OptionsObj>();

            while ((variant = optionSheet.NextRow()) != null)
            {
                OptionsObj options = new OptionsObj();
                options.ProductCode =  variant.Cell("ProductCode");
                options.Size =  variant.Cell("Size");
                options.Color =  variant.Cell("Color");
                productVariantMapping.Add(variant.Cell("VariationCode"), options);
            }


            RowData orderRow = null;
            var maxOrders = 50000;
            var orderCount = 1;
            while ((orderRow = orderSheet.NextRow()) != null & orderCount < maxOrders)
            {
                orderCount++;
                OrderModel orderModel = new OrderModel();
                orderModel.Order = new Order();
                
                // this order number is the Volusion order number and needs to be added via the API to "ExternalID"                
                orderModel.Order.ExternalId = orderRow.Cell("Order Number");
                orderModel.Order.OrderNumber = orderRow.Cell("Order Number").ToInt32(0);


                orderModel.Order.CustomerAccountId = orderRow.Cell("Customer Account ID").ToInt32(0);
                orderModel.Order.IpAddress = orderRow.Cell("IP Address"); // is this right?

                orderModel.Order.AcceptedDate = orderRow.Cell("Submitted On").ToDateTime(DateTime.Now);
                orderModel.Order.SubmittedDate = orderRow.Cell("Submitted On").ToDateTime(DateTime.Now);

                // this was needed in order to set the read-only SubmittedDate
                if (orderModel.Order.SubmittedDate != null) {
                    orderModel.Order.AuditInfo = new AuditInfo() { CreateBy = "volusion_import", CreateDate = orderModel.Order.SubmittedDate, UpdateBy = "volusion_import", UpdateDate = orderModel.Order.SubmittedDate };
                }

                orderModel.Order.BillingInfo = new BillingInfo();
                //orderModel.Order.BillingInfo.IsSameBillingShippingAddress = true; // how to tell?
                orderModel.Order.BillingInfo.PaymentType = orderRow.Cell("Payment Type"); // Is this correct?
                orderModel.Order.BillingInfo.StoreCreditCode = "No"; // Is this correct?

                orderModel.Order.BillingInfo.BillingContact = new Contact();
                //orderModel.Order.BillingInfo.BillingContact.Id = null; 
                orderModel.Order.BillingInfo.BillingContact.FirstName = orderRow.Cell("Billing Contact First Name");
                orderModel.Order.BillingInfo.BillingContact.LastNameOrSurname =
                    orderRow.Cell("Billing Contact Last Name");
                orderModel.Order.BillingInfo.BillingContact.MiddleNameOrInitial = string.Empty;
                    // where does this come from?
                orderModel.Order.BillingInfo.BillingContact.Email = orderRow.Cell("Billing Contact Email");

                orderModel.Order.BillingInfo.BillingContact.PhoneNumbers = new Phone();
                orderModel.Order.BillingInfo.BillingContact.PhoneNumbers.Home =
                    orderRow.Cell("Billing Contact Home Phone");

                orderModel.Order.BillingInfo.BillingContact.Address = new Address();
                orderModel.Order.BillingInfo.BillingContact.Address.Address1 = orderRow.Cell("Billing Address1");
                orderModel.Order.BillingInfo.BillingContact.Address.Address2 = orderRow.Cell("Billing Address2");
                orderModel.Order.BillingInfo.BillingContact.Address.Address3 = orderRow.Cell("Billing Address3");
                orderModel.Order.BillingInfo.BillingContact.Address.Address4 = orderRow.Cell("Billing Address4");
                orderModel.Order.BillingInfo.BillingContact.Address.CityOrTown = orderRow.Cell("Billing City");
                orderModel.Order.BillingInfo.BillingContact.Address.StateOrProvince = orderRow.Cell("Billing State");
                orderModel.Order.BillingInfo.BillingContact.Address.CountryCode = orderRow.Cell("Billing Country");
                orderModel.Order.BillingInfo.BillingContact.Address.AddressType = "Residential";
                orderModel.Order.BillingInfo.BillingContact.Address.PostalOrZipCode = orderRow.Cell("Billing Postal Code");

                ////orderModel.Order.BillingInfo.Card = new PaymentCard();
                ////orderModel.Order.BillingInfo.Card.PaymentOrCardType = orderRow.Cell("Payment Type");
                ////orderModel.Order.BillingInfo.Card.CardNumberPartOrMask = "notavailable";
                ////orderModel.Order.BillingInfo.Card.ExpireMonth = 1; // not available
                ////orderModel.Order.BillingInfo.Card.ExpireYear = 2015; // not available
                ////orderModel.Order.BillingInfo.Card.IsCardInfoSaved = false; // not available
                ////orderModel.Order.BillingInfo.Card.IsUsedRecurring = false; // not available
                ////orderModel.Order.BillingInfo.Card.NameOnCard = "not available";
                ////orderModel.Order.BillingInfo.Card.PaymentServiceCardId = string.Empty; // not available

                // Shippment data
                var shipment = new Shipment();
                shipment.Id = null; // where does this come from?
                shipment.Cost = orderRow.Cell("Shipping Total").ToDecimal(new decimal(0.00));
                shipment.CurrencyCode = DefaultCurrencyCode;
                shipment.ShippingMethodCode = orderRow.Cell("Shipping Method");
                shipment.SignatureRequired = false;
                shipment.TrackingNumber = "not available"; // where does this come from?

                // Destination Address
                shipment.DestinationAddress = new Contact();
                //shipment.DestinationAddress.Id = null; // need to hook up later
                shipment.DestinationAddress.FirstName = orderRow.Cell("Shipping Contact First Name");
                //shipment.DestinationAddress.MiddleNameOrInitial = ""; // where does this come from?
                shipment.DestinationAddress.LastNameOrSurname = orderRow.Cell("Shipping Contact Last Name");
                shipment.DestinationAddress.Email = orderRow.Cell("Shipping Contact Email").ToLower();
                shipment.DestinationAddress.PhoneNumbers = new Phone()
                {
                    Home = orderRow.Cell("Shipping Contact Home Phone")
                };
                shipment.DestinationAddress.CompanyOrOrganization = ""; // where does this come from?

                shipment.DestinationAddress.Address = new Address();
                shipment.DestinationAddress.Address.Address1 = orderRow.Cell("Shipping Address1");
                shipment.DestinationAddress.Address.Address2 = orderRow.Cell("Shipping Address2");
                shipment.DestinationAddress.Address.Address3 = orderRow.Cell("Shipping Address3");
                shipment.DestinationAddress.Address.Address4 = orderRow.Cell("Shipping Address4");
                shipment.DestinationAddress.Address.AddressType =
                    (!string.IsNullOrEmpty(shipment.DestinationAddress.CompanyOrOrganization))
                        ? "Commercial"
                        : "Residential";
                shipment.DestinationAddress.Address.CityOrTown = orderRow.Cell("Shipping City");
                shipment.DestinationAddress.Address.StateOrProvince = orderRow.Cell("Shipping State");
                shipment.DestinationAddress.Address.CountryCode = orderRow.Cell("Shipping Country");
                shipment.DestinationAddress.Address.PostalOrZipCode = orderRow.Cell("Shipping Postal Code");
                shipment.DestinationAddress.Address.IsValidated = true;
                // does making this true cause Mozu to skip validation?
                shipment.OriginAddress = shipment.DestinationAddress;

                

                // Add to order
                orderModel.Order.Shipments = new List<Shipment>() { shipment };

                // Order Totals
                orderModel.Order.CurrencyCode = DefaultCurrencyCode; // where does this come from?
                orderModel.Order.AmountAvailableForRefund = new Decimal(0.00);
                orderModel.Order.AmountRemainingForPayment = new Decimal(0.00);
                orderModel.Order.DiscountTotal = orderRow.Cell("Discount Total").ToDecimal(new decimal(0.00));
                orderModel.Order.DiscountedSubtotal = orderRow.Cell("Discounted Sub Total").ToDecimal(new decimal(0.00));
                orderModel.Order.DiscountedTotal = orderModel.Order.DiscountedSubtotal; // where does this come from
                orderModel.Order.FeeTotal = orderRow.Cell("Fee Total").ToDecimal((Decimal)0.00);
                orderModel.Order.HandlingAmount = orderRow.Cell("Handling Amount").ToDecimal(new decimal(0.00));
                orderModel.Order.HandlingTaxTotal = orderRow.Cell("Handling Tax").ToDecimal(new decimal(0.00));
                orderModel.Order.HandlingTotal = orderModel.Order.HandlingAmount + orderModel.Order.HandlingTaxTotal;
                orderModel.Order.ItemTaxTotal = orderRow.Cell("Item Tax").ToDecimal((Decimal)0.00);
                orderModel.Order.ShippingTotal = orderRow.Cell("Shipping Total").ToDecimal((Decimal)0.00);
                orderModel.Order.ShippingSubTotal = orderModel.Order.ShippingTotal;
                //orderModel.Order.ShippingTaxTotal = (Decimal) 0.00;
                orderModel.Order.Subtotal = orderRow.Cell("Sub Total").ToDecimal((Decimal)0.00);
                orderModel.Order.TaxTotal = orderRow.Cell("Tax").ToDecimal((Decimal)0.00);
                orderModel.Order.Total = orderRow.Cell("Total").ToDecimal((Decimal)0.00);
                orderModel.Order.TotalCollected = orderModel.Order.Total.Value;

                // Other
                orderModel.Order.Id = null;   // can't hard code this
                orderModel.Order.Version = "1";
                orderModel.Order.IsImport = true;
                orderModel.Order.ImportDate = DateTime.Now;
                orderModel.Order.SiteId = Context.ApiContext.SiteId; // is this necessary?
                orderModel.Order.ChannelCode = "Volusion"; // where does this come from?
                orderModel.Order.Email = orderRow.Cell("Billing Contact Email").ToLower();
                orderModel.Order.HasDraft = false;
                orderModel.Order.IsDraft = false;
                orderModel.Order.IsEligibleForReturns = false;
                orderModel.Order.IsTaxExempt = false;
                // is this Order Type
                orderModel.Order.CustomerInteractionType = "Website";

                //orderModel.Order.AvailableActions = new List<string>(); // no available actions for order history
                orderModel.Order.Adjustment = null; // not available
                orderModel.Order.ClosedDate = orderModel.Order.AcceptedDate; // not available

                // set completedStatus, fullfillmentStatus and paymentStatus dependent on 
                var completedStatus = orderRow.Cell("Status");
                var fulfillmentStatus = orderRow.Cell("Shipment Status");
                var paymentStatus = orderRow.Cell("Payment Status");
                var returnStatus = orderRow.Cell("Return Status");

                // per Molly, hard code these statuses
                // per Sanjay, if the order status is not Completed or Cancelled, then the order is checked against inventory
                string commentText = "";
                if (completedStatus.ToLower() == "cancelled")
                {
                    orderModel.Order.Status = "Cancelled";
                    orderModel.Order.FulfillmentStatus = "NotFulfilled";
                    commentText = "Imported From Volusion Order Number: " + orderModel.Order.ExternalId;

                }
                else if (completedStatus.ToLower() == "partially returned")
                {
                    orderModel.Order.Status = "Completed";
                    orderModel.Order.FulfillmentStatus = "Fulfilled";
                    commentText = "Imported From Volusion Order Number: " + orderModel.Order.ExternalId + " Order was Partially Returned.";

                }
                else if (completedStatus.ToLower() == "returned")
                {
                    orderModel.Order.Status = "Cancelled";
                    orderModel.Order.FulfillmentStatus = "NotFulfilled";
                    commentText = "Imported From Volusion Order Number: " + orderModel.Order.ExternalId + " Order was Returned.";

                }
                else 
                {
                    orderModel.Order.Status = "Completed";
                    orderModel.Order.FulfillmentStatus = "Fulfilled";
                    commentText = "Imported From Volusion Order Number: " + orderModel.Order.ExternalId;
                }

                orderModel.Order.PaymentStatus = "Paid";
                orderModel.Order.ReturnStatus = returnStatus;

                orderModel.Order.Notes = new List<OrderNote>()
                {
                    new OrderNote()
                    {
                        AuditInfo = new AuditInfo(),
                        Id = "",
                        Text = commentText
                    }
                };


                // Link up to an existing account and contact
                if (!_existingAccounts.ContainsKey(orderModel.Order.Email))
                {
                   // throw new Exception(
                     //   string.Format("Unable to import orderhistory for {0} because customer does not exist {1}",
                       //     orderModel.Order.OrderNumber,
                         //   orderModel.Order.Email));
                    ReportProgress(string.Format("Unable to import orderhistory for \"{0}\" because customer \"{1}\" does not exist.", orderModel.Order.ExternalId, orderModel.Order.Email));
                }

                else { 

                    // reset the user account ID to the one we look up based on the matched email
                    orderModel.Order.CustomerAccountId = _existingAccounts[orderModel.Order.Email].Id;

                    var account = _existingAccounts[orderModel.Order.Email];
                    //orderModel.Order.CustomerAccountId = account.Id;

                    // Find the billing contact or the first one whichever we can get.

                    // TODO some customers don't have contact information (address)
       //             CustomerContact billingContact = account.Contacts.FirstOrDefault(contact => contact.Types.Contains(new ContactType() { IsPrimary = true, Name = "Billing" }))
         //               ?? account.Contacts[0];

                    //orderModel.Order.BillingInfo.BillingContact.Id = billingContact.Id; // where does this come from?
                    //orderModel.Order.Shipments[0].DestinationAddress.Id;// where does this come from?

                    // Other not available or not set
                    //orderModel.Order.CustomerTaxId = ""; // not available
                    //orderModel.Order.ExpirationDate; // is this the order expiration date or the cc expiration date?
                    //orderModel.Order.ExternalId; // where does this come from?
                    orderModel.Order.LocationCode = "homebase"; // where does this come from?
                    //orderModel.Order.OriginalCartId = ""; // where does this come from?
                    //orderModel.Order.Packages;
                    //orderModel.Order.ParentReturnId;
                    //orderModel.Order.Payments;
                    //orderModel.Order.Pickups;
                    //orderModel.Order.ShippingDiscounts;
                    //orderModel.Order.SourceDevice; // where does this come from?
                    //orderModel.Order.TenantId = 6466;    // clean test
                    //orderModel.Order.TenantId = 7927;   // order import test
                    orderModel.Order.TenantId = 6653;    // production
                    //orderModel.Order.ValidationResults;
                    //orderModel.Order.Version;
                    //orderModel.Order.VisitId;
                    //orderModel.Order.WebSessionId;

                    // Add required collections -- even if they are left empty
                    orderModel.Order.Attributes = new List<OrderAttribute>();
                    //orderModel.Order.AuditInfo= new AuditInfo();
                    orderModel.Order.AvailableActions = new List<string>();
                    //orderModel.Order.BillingInfo = new BillingInfo(); // no billing info will be imported
                    orderModel.Order.ChangeMessages = new List<ChangeMessage>();
                    orderModel.Order.CouponCodes = new List<string>();
                    orderModel.Order.FulfillmentInfo = new FulfillmentInfo();
                    orderModel.Order.InvalidCoupons = new List<InvalidCoupon>();
                    orderModel.Order.OrderDiscounts = new List<AppliedDiscount>();
                    orderModel.Order.Packages = new List<Package>();
                    orderModel.Order.Payments = new List<Payment>();
                    orderModel.Order.Pickups = new List<Pickup>();
                    orderModel.Order.ShippingDiscounts = new List<ShippingDiscount>();
                    orderModel.Order.ShopperNotes = new ShopperNotes();
                    orderModel.Order.ValidationResults = new List<OrderValidationResult>();

                    orderModel.Order.FulfillmentInfo.ShippingMethodCode = shipment.ShippingMethodCode;
                    orderModel.Order.FulfillmentInfo.ShippingMethodName = shipment.ShippingMethodCode;

                    orderModel.Order.FulfillmentInfo.FulfillmentContact = shipment.DestinationAddress;

                    orderModel.Order.Items = new List<OrderItem>();

                    IList<RowData> detailData = lineItemsSheet.GetRows(orderModel.Order.OrderNumber.ToString());
                    var itemCount = 0;
                    foreach (var row in detailData)
                    {
                        var lineItem = new OrderItem();
                        lineItem.Id = itemCount.ToString();
                        lineItem.DiscountTotal = row.Cell("Discount Total").ToDecimal(new decimal(0.00));
                        // detail.DiscountedTotal; 
                        lineItem.ExtendedTotal = row.Cell("Total").ToDecimal(new decimal(0.00));
                        //detail.FeeTotal;
                        //detail.FulfillmentLocationCode;
                        //detail.FulfillmentMethod;
                        lineItem.IsRecurring = false;
                        lineItem.IsTaxable = row.Cell("Is Taxable").ToBoolean(true);
                        lineItem.ItemTaxTotal = row.Cell("Taxable Total").ToDecimal(new decimal(0.00));
                        lineItem.LocaleCode = "en-US";
                        //detail.OriginalCartItemId;

                        lineItem.Product = new Product();

                        //lineItem.Product.Price = row.Cell("Total").ToDecimal(new ProductPrice());

                        string variationCode = row.Cell("Product Variation Code");
                        if (variationCode.Contains("-"))
                        {
                            variationCode = variationCode.Substring(variationCode.LastIndexOf("-") + 1, variationCode.Length - variationCode.LastIndexOf("-") - 1);

                        }
                        lineItem.Product.VariationProductCode = variationCode;
                        string productName = row.Cell("Product Name");
                        lineItem.Product.Name = productName;


                        if (productVariantMapping.ContainsKey(lineItem.Product.VariationProductCode))
                        {
                            // the product code is not on the spreadsheet.  We need to look it up given the variation code
                            //lineItem.Product.ProductCode = row.Cell("Product Code");
                            setProductCode(lineItem, productVariantMapping);

                            // detail.ProductDiscount;
                            //detail.ProductDiscounts;

                            setProductOptions(lineItem, productVariantMapping);
                        }
                        else
                        {
                            // hard code ProductCode to 0000
                            lineItem.Product.ProductCode = "0000";

                            // set size and color to unknown
                            setColorAndSize(lineItem, "unknown", "unknown");
                            
                            
                            //ReportProgress(string.Format("Unable to add line item for product \"{1}\" with ID of \"{0}\" because item doesn't exist in import spreasheet.", orderModel.Order.ExternalId, lineItem.Product.VariationProductCode));

                        }

                        lineItem.Quantity = row.Cell("Quantity").ToInt32(0);
                        //detail.ShippingDiscounts;
                        //detail.ShippingTotal;
                        lineItem.Subtotal = row.Cell("Sub Total").ToDecimal(new decimal(0.00));
                        lineItem.TaxableTotal = row.Cell("Taxable Total").ToDecimal(new decimal(0.00));
                        lineItem.Total = row.Cell("Total").ToDecimal(new decimal(0.00));
                        // detail.UnitPrice;

                        if (lineItem.Quantity == 0)
                        {

//                                ReportProgress(string.Format("Unable to add line item for product \"{1}\" with ID of \"{0}\" because item quantity is zero.", orderModel.Order.ExternalId, lineItem.Product.VariationProductCode));
                            lineItem.Quantity = 1;
                            orderModel.Order.Status = "Cancelled";
                            orderModel.Order.FulfillmentStatus = "NotFulfilled";


                        }

                        orderModel.Order.Items.Add(lineItem);
                        itemCount++;
                    


                    }

                    // only add the order Model if at least one line item has a quantity > 0
                    if (itemCount > 0)
                    {
                        ordersToImport.Add(orderModel);
                    }
                }
            }


            return ordersToImport;
        }

        private void setProductCode(OrderItem lineItem, Dictionary<string, OptionsObj> productMapping)
        {

            string variationCode = lineItem.Product.VariationProductCode;

            lineItem.Product.ProductCode = productMapping[variationCode].ProductCode;


        }



        private void setProductOptions(OrderItem lineItem, Dictionary<string, OptionsObj> productMapping)
        {


            string variationCode = lineItem.Product.VariationProductCode;

            string size = productMapping[variationCode].Size;
            string color = productMapping[variationCode].Color;

            setColorAndSize(lineItem, size, color);

        
        }


        private void setColorAndSize(OrderItem lineItem, string color, string size)
        {

            // need to remove spaces from size and color
            string cleanedSize = size.StripInvalidValueCharacters();
            string cleanedColor = color.StripInvalidValueCharacters();

            lineItem.Product.Options = new List<ProductOption>();
            var prodSize = new ProductOption();
            prodSize.Name = "Color";
            prodSize.Value = cleanedColor;
            prodSize.AttributeFQN = "tenant~Color";
            prodSize.StringValue = color;
            lineItem.Product.Options.Add(prodSize);
            var prodColor = new ProductOption();
            prodColor.Name = "Size";
            prodColor.Value = cleanedSize;
            prodColor.AttributeFQN = "tenant~Size";
            prodColor.StringValue = size;
            lineItem.Product.Options.Add(prodColor);


        }
        private void CreateOrder(OrderModel orderModel)
        {
            // Create Order History from the model
            var orderResource = new OrderResource(Context.ApiContext);

            // get a sample existing order... this is just for debug/testing purposes
            var testOrder = GetTestOrder();


            // TODO handle exceptions from here and log them.  There could be products which no longer exist or invalid colors

            // Create order History  
            ReportProgress(string.Format("Starting order import for {4} items.  The first variation is \"{0}\" of Product Code \"{1}\" with color of \"{2}\" and size of \"{3}\".", orderModel.Order.Items[0].Product.VariationProductCode, orderModel.Order.Items[0].Product.ProductCode, orderModel.Order.Items[0].Product.Options[0].StringValue, orderModel.Order.Items[0].Product.Options[1].StringValue, orderModel.Order.Items.Count()));

            orderModel.Order = orderResource.CreateOrder(orderModel.Order);
        }


        private Order GetTestOrder()
        {
            var orderResource = new OrderResource(Context.ApiContext);
            var orders = orderResource.GetOrders(0, 1);
            return orders.Items.FirstOrDefault(); // might return null
        }
    }
}
