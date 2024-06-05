using InvoiceGen.Api;
using InvoiceGen.Api.Models;

namespace InvoiceGen.Tests
{
    [TestClass]
    public class PdfTests
    {
        [TestMethod]
        public void GenerateAndShowPdf()
        {
            PdfGenerator generator = new PdfGenerator();
            Invoice invoice = new Invoice
            {
                InvoiceNumber = 1,
                IssueDate = DateTime.Now,
                SellerAddress = new Address
                {
                    CompanyName = "Tom Visser",
                    City = "Aalden",
                    Email = "tomvisser@craziac.com",
                    Phone = "06-15528400",
                    Zip = "7854 TC",
                    Street = "Nooitgedacht 2"
                },
                CustomerAddress = new Address
                {
                    CompanyName = "Thie",
                    Street = "Oud Aalden 3",
                    City = "Aalden",
                    Zip = "7855 AA",
                    Email = "thie@nijenbanning.com",
                    Phone = "06"
                },
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Name = "Diverse werkzaamheden",
                        Price = 25.50M,
                        Quantity = 10
                    },
                    new OrderItem
                    {
                        Name = "Voorrijkosten",
                        Price = 0.25M,
                        Quantity = 45
                    }
                }
            };

            generator.CreatePdf(invoice, @"c:\users\tomderidder\downloads\test.pdf");
            Assert.AreEqual(true, true);
        }
    } // end c
} // end ns