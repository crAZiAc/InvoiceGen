using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvoiceGen.Api.Controllers
{
    public class AddressController
    {
        protected IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        } // ct

        public async Task<List<Address>> GetAddresses()
        {
            return await _addressService.GetAddressesAsync();
        } // end f

        /// <summary>
        /// Gets Adress
        /// </summary>
        /// <param name="ID">This is the Address Identifier, technically a GUID string: the rowKey from the Azure Table</param>
        /// <returns>The requested address</returns>
        public async Task<Address> GetAddress(string ID)
        {
            Address address = await _addressService.GetAddressAsync(ID);
            if (address != null)
            {
                return address;
            }
            else
            {
                return null;
            }
        } // end f

        public async Task<Address> AddAddress(Address address)
        {
            return await _addressService.AddAddressAsync(address);
        } // end f

        public async Task UpdateAddress(Address address)
        {
            await _addressService.UpdateAddressAsync(address);
        } // end f

        /// <summary>
        /// Deletes the address
        /// </summary>
        /// <param name="ID">This is the Address Identifier, technically a GUID string: the rowKey from the Azure Table</param>
        /// <returns>Result for the operation</returns>
        public async Task DeleteAddress(string ID)
        {
            await _addressService.DeleteAddressAsync(ID);
        } // end f

    } // end c
} // end ns
