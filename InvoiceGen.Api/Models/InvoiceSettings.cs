using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Models
{
    public class InvoiceSettings: BaseEntity
    {
        public InvoiceSettings() 
        {
            this.PartitionKey = "InvoiceSettings";
        }

        public string SelectedSellerAddressId { get; set; }
    } // end c
} // end ns
