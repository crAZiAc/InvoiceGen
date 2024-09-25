using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization;

namespace InvoiceGen.Api.Models
{
    public class Invoice : BaseEntity, ITableEntity
    {
        public Invoice()
        {
            this.PartitionKey = "Invoice";
            this.Paid = false;
        }
        public int InvoiceNumber { get; set; }
        public DateTime? IssueDate { get; set; }

        public string SellerAddressId { get; set; }
        public string CustomerAddressId { get; set; }
        public string Name { get; set; }

        public bool Paid { get; set; }

        [IgnoreDataMember]
        public Address SellerAddress { get; set; }
        [IgnoreDataMember]
        public Address CustomerAddress { get; set; }

        [IgnoreDataMember]
        public List<OrderItem> Items { get; set; }
        public string Comments { get; set; }



        [IgnoreDataMember]
        public string InvoiceId
        {
            get
            {
                return $"F{InvoiceNumber:00000}";
            }
        }

        [IgnoreDataMember]
        public double TotalAmount
        {
            get
            {
                return Math.Round(Items.Sum(i => i.Quantity * i.Price), 2);
            }
        }
        [IgnoreDataMember]
        public double TotalAmountWithVat
        {
            get
            {
                return Math.Round(Items.Sum(i => i.Quantity * i.Price + i.VatAmount), 2);
            }
        }
        [IgnoreDataMember]
        public double TotalVat
        {
            get
            {
                return Math.Round(Items.Sum(i => i.VatAmount), 2);
            }
        }

        [IgnoreDataMember]
        public string PaidInText
        {
            get
            {
                if (this.Paid)
                {
                    return "Ja";
                }
                else
                {
                    return "Nee";
                }
            }
        }

    } // end c

   
} // end ns
