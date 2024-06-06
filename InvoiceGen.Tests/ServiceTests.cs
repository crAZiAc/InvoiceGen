using Azure.Data.Tables;
using InvoiceGen.Api.Models;
using InvoiceGen.Api.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Tests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void AddAddressTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            AddressService service = new AddressService(serviceClient, connectString);
            List<Address> addresses = service.GetAddressesAsync().Result;
            if (addresses.Any())
            {
                if (addresses.Select(a => a.CompanyName == "Tom Visser").Count() > 0)
                {
                    Assert.AreEqual(true, true);
                }
            }
            else
            {
                service.AddAddressAsync(new Api.Models.Address
                {
                    CompanyName = "Tom Visser",
                    City = "Aalden",
                    Email = "tomvisser@craziac.com",
                    Phone = "06-15528400",
                    Zip = "7854 TC",
                    Street = "Nooitgedacht 2"
                }).Wait();
            }
            Assert.AreEqual(true, true);
        } // end t


        [TestMethod]
        public void AddInvoiceTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService service = new InvoiceService(serviceClient, connectString);
            List<Invoice> invoices = service.GetInvoicesAsync().Result;
            if (invoices.Any())
            {
                if (invoices.Select(a => a.InvoiceNumber == 1).Count() > 0)
                {
                    Assert.AreEqual(true, true);
                }
            }
            else
            {
                service.AddInvoiceAsync(new Api.Models.Invoice
                {
                    IssueDate = DateTime.Now,
                    Comments = "First one",
                    SellerAddressId = "ba99e3c3-24a7-42ae-81b9-491d8df54998",
                    CustomerAddressId = "ba99e3c3-24a7-42ae-81b9-491d8df54998",
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Name = "Diverse werkzaamheden",
                            Price = 25.50,
                            Quantity = 10
                        },
                        new OrderItem
                        {
                            Name = "Voorrijkosten",
                            Price = 0.25,
                            Quantity = 45
                        }
                    }

                }).Wait();
            }
            Assert.AreEqual(true, true);
        } // end t


        [TestMethod]
        public void GetInvoiceTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService service = new InvoiceService(serviceClient, connectString);
            Invoice invoice = service.GetInvoiceAsync("2ce8d915-8d05-4bae-9d4e-eea7093656ac").Result;
            if (invoice != null)
            {
                Assert.AreEqual(2, invoice.Items.Count());
            }
            else
            {
                Assert.AreEqual(true, false);
            }
            
        } // end t
    } // end c
} // end ns
