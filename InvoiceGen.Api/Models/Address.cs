using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Models
{
    public class Address : BaseEntity, ITableEntity
    {
        public Address()
        {
            this.PartitionKey = "Address";
        }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    } // end c

    public class SelectedSellerId
    {
        public string Id { get; set; }
    } // end c

} // end ns
