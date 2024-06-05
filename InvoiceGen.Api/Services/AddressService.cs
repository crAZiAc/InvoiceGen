using Azure.Data.Tables;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Services
{
    public class AddressService : IAddressService
    {
        private TableClient _addressContainer;
        private string _connectionString;

        public AddressService(TableServiceClient dbClient, string connectionString)
        {
            _connectionString = connectionString;
            this._addressContainer = dbClient.GetTableClient("Address");
        }
        public string StorageConnectionString
        {
            get
            {
                return _connectionString;
            }
        } // end prop

        public async Task<Address> AddAddressAsync(Address address)
        {
            try
            {
                address.RowKey = Guid.NewGuid().ToString();
                await this._addressContainer.AddEntityAsync<Address>(address);
                return address;
            }
            catch (Exception ex)
            {
                return null;
            }
        } // end f

        public async Task DeleteAddressAsync(string rowKey)
        {
            await this._addressContainer.DeleteEntityAsync("Address", rowKey);
        }

        public async Task<Address> GetAddressAsync(string rowKey)
        {
            var query = from c in _addressContainer.Query<Address>()
                        where c.RowKey == rowKey
                        where c.PartitionKey == "Address"
                        select c;
            try
            {
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        } // end f

        public async Task<List<Address>> GetAddressesAsync()
        {
            var query = from c in _addressContainer.Query<Address>()
                        where c.PartitionKey == "Address"
                        select c;
            try
            {
                return query.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateAddressAsync(Address address)
        {
            try
            {
                await this._addressContainer.UpsertEntityAsync<Address>(address);
            }
            catch (Exception ex)
            {

            }
        } // end f
      
    } // end c
} // end ns
