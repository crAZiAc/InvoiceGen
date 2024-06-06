using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceGen.Api.Models;

namespace InvoiceGen.Api.Interfaces
{
    public interface IInvoiceService
    {
        string StorageConnectionString { get; }
        Task<List<Invoice>> GetInvoicesAsync();
        Task<Invoice> GetInvoiceAsync(string rowKey);
        Task AddInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(string rowKey);
        Task DeleteOrderItemAsync(string rowKey);
    } // end i
} // end cs
