using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InvoiceGen.Api.Models;

namespace InvoiceGen.Api.Interfaces
{
    public interface IAddressService
    {
        string StorageConnectionString { get; }

        Task<List<Address>> GetAddressesAsync();
        Task<Address> GetAddressAsync(string rowKey);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(string rowKey);

    } // end i 
} // end ns
