using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Models
{
    public class OrderItem : BaseEntity, ITableEntity
    {
        public OrderItem()
        {
            this.PartitionKey = "OrderItem";
        }
        public string RelatedInvoiceId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        [IgnoreDataMember]
        public double VatAmount
        {
            get
            {
                return Price * Quantity * Constants.VAT_AMOUNT;
            }
        }
        [IgnoreDataMember]
        public double Amount
        {
            get
            {
                return Price * Quantity;
            }
        }
    } // end c
} // end ns
