using Mozu.Api.Contracts.Customer;
using System.Collections.Generic;

namespace MozuImport.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Contacts = new List<CustomerContact>();
        }

        public CustomerAccount Account { get; set; }
        public List<CustomerContact> Contacts { get; set; }
        public string Password { get; set; }
        public bool Delete { get; set; }
    }
}
