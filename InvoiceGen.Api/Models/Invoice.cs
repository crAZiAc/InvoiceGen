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
        }
        public int InvoiceNumber { get; set; }
        [Required(ErrorMessage = "Factuurdatum verplicht.")]
        public DateOnly? IssueDate { get; set; }

        public string SellerAddressId { get; set; }
        public string CustomerAddressId { get; set; }

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

    } // end c

    public class OrderItem : BaseEntity, ITableEntity
    {
        public OrderItem()
        {
            this.PartitionKey = "OrderItem";
        }
        public string RelatedInvoiceId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
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
        public object Email { get; set; }
        public string Phone { get; set; }
    } // end c

} // end ns
