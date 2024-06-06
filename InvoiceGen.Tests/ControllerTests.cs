using Azure.Data.Tables;
using InvoiceGen.Api.Controllers;
using InvoiceGen.Api.Models;
using InvoiceGen.Api.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Tests
{
    [TestClass]
    public class ControllerTests
    {
        #region Invoice Controller Tests
        [TestMethod]
        public void AddInvoiceTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService invoiceService = new InvoiceService(serviceClient, connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            InvoiceController controller = new InvoiceController(invoiceService, addressService);

            Invoice newInvoice = new Invoice
            {
                CustomerAddressId = "ba99e3c3-24a7-42ae-81b9-491d8df54998",
                SellerAddressId = "ba99e3c3-24a7-42ae-81b9-491d8df54998",
                IssueDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Name = "Werk",
                        Price = 25.22,
                        Quantity = 30
                    },
                    new OrderItem
                    {
                        Name = "Reizen",
                        Price = 1.13,
                        Quantity = 67
                    }
                }
            };

            var returnInvoice = controller.AddInvoice(newInvoice).Result;
            if (returnInvoice != null)
            {
                Assert.AreEqual(2, returnInvoice.Items.Count());
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t

        [TestMethod]
        public void GetInvoiceTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService invoiceService = new InvoiceService(serviceClient, connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            InvoiceController controller = new InvoiceController(invoiceService, addressService);

            var invoices = controller.GetInvoices().Result;
            var invoiceCheck = invoices.Where(i => i.InvoiceNumber == 2);
            if (invoiceCheck.Any())
            {
                var returnInvoice = controller.GetInvoice(invoiceCheck.FirstOrDefault().RowKey).Result;
                if (returnInvoice != null)
                {
                    Assert.AreEqual(2, returnInvoice.Items.Count());
                }
                else
                {
                    Assert.AreEqual(true, false);
                }
            }
            else
            {
                Assert.AreEqual(true, false);
            }

        } // end t

        [TestMethod]
        public void GetInvoicesTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService invoiceService = new InvoiceService(serviceClient, connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            InvoiceController controller = new InvoiceController(invoiceService, addressService);

            var invoices = controller.GetInvoices().Result;
            if (invoices != null)
            {
                Assert.AreEqual(2, invoices.Count);
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t

        [TestMethod]
        public void UpdateInvoiceTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService invoiceService = new InvoiceService(serviceClient, connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            InvoiceController controller = new InvoiceController(invoiceService, addressService);


            var invoices = controller.GetInvoices().Result;
            var invoiceCheck = invoices.Where(i => i.InvoiceNumber == 2);
            if (invoiceCheck.Any())
            {
                var returnInvoice = controller.GetInvoice(invoiceCheck.FirstOrDefault().RowKey).Result;
                if (returnInvoice != null)
                {
                    if (returnInvoice.Items.Count() >= 3)
                    {
                        Assert.AreEqual(true, true);
                    }
                    else
                    {
                        returnInvoice.Items.Add(new OrderItem
                        {
                            Name = "Voorrijden",
                            Price = 10.50,
                            Quantity = 1
                        });

                        Invoice check = controller.UpdateInvoice(returnInvoice).Result;
                        Assert.AreEqual(3, check.Items.Count());
                    }
                }
                else
                {
                    Assert.AreEqual(true, false);
                }
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t

        [TestMethod]
        public void DeleteInvoiceTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            InvoiceService invoiceService = new InvoiceService(serviceClient, connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            InvoiceController controller = new InvoiceController(invoiceService, addressService);

            var invoices = controller.GetInvoices().Result;
            var invoiceCheck = invoices.Where(i => i.InvoiceNumber == 2);
            if (invoiceCheck.Any())
            {
                var returnInvoice = controller.GetInvoice(invoiceCheck.FirstOrDefault().RowKey).Result;
                if (returnInvoice != null)
                {
                    controller.DeleteInvoice(invoiceCheck.FirstOrDefault().RowKey).Wait();
                }
                else
                {
                    Assert.AreEqual(true, false);
                }
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t
        #endregion
        #region Address Controller Tests
        [TestMethod]
        public void GetAddressesTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            AddressController controller = new AddressController(addressService);

            var addresses = controller.GetAddresses().Result;
            if (addresses.Any())
            {
                Assert.AreEqual(2, addresses.Count());
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t

        [TestMethod]
        public void AddAddressTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            AddressController controller = new AddressController(addressService);

            var addresses = controller.GetAddresses().Result;
            if (addresses.Any())
            {
                var checkAddress = addresses.Where(a => a.CompanyName == "Thie");
                if (checkAddress.Any())
                {
                    Assert.AreEqual(true, true);
                }
                else
                {
                    Address address = new Address
                    {
                        CompanyName = "Thie",
                        Street = "Oud Aalden 3",
                        City = "Aalden",
                        Zip = "7855 AA",
                        Email = "thie@nijenbanning.com",
                        Phone = "06"
                    };

                    var returnAddress = controller.AddAddress(address).Result;
                    if (returnAddress != null)
                    {
                        Assert.AreEqual("Thie", returnAddress.CompanyName);
                    }
                    else
                    {
                        Assert.AreEqual(true, false);
                    }
                }
            }
        } // end t

        [TestMethod]
        public void UpdateAddressTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            AddressController controller = new AddressController(addressService);

            var addresses = controller.GetAddresses().Result;
            if (addresses.Any())
            {
                var checkAddress = addresses.Where(a => a.CompanyName == "Thie").FirstOrDefault();
                if (checkAddress != null)
                {
                    checkAddress.Phone = "06-15125455";
                    controller.UpdateAddress(checkAddress).Wait();

                    // Check update
                    var returnAddress = controller.GetAddress(checkAddress.RowKey).Result;
                    if (returnAddress != null)
                    {
                        Assert.AreEqual("06-15125455", returnAddress.Phone);
                    }
                    else
                    {
                        Assert.AreEqual(true, false);
                    }
                }
                else
                {
                    Assert.AreEqual(true, false);
                }
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t

        [TestMethod]
        public void DeleteAddressTest()
        {
            string connectString = "UseDevelopmentStorage=true";
            var serviceClient = new TableServiceClient(connectString);
            AddressService addressService = new AddressService(serviceClient, connectString);
            AddressController controller = new AddressController(addressService);

            var addresses = controller.GetAddresses().Result;
            if (addresses.Any())
            {
                var checkAddress = addresses.Where(a => a.CompanyName == "Thie").FirstOrDefault();
                if (checkAddress != null)
                {
                    controller.DeleteAddress(checkAddress.RowKey).Wait();

                    // Check update
                    var returnAddress = controller.GetAddress(checkAddress.RowKey).Result;
                    if (returnAddress != null)
                    {
                        Assert.AreEqual(true, false);
                    }
                    else
                    {
                        Assert.AreEqual(true, true);
                    }
                }
                else
                {
                    Assert.AreEqual(true, false);
                }
            }
            else
            {
                Assert.AreEqual(true, false);
            }
        } // end t
        #endregion
    } // end c
} // end ns
