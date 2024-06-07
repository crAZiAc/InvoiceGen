using InvoiceGen.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Interfaces
{
    public interface ISettingsService
    {
        string StorageConnectionString { get; }
        Task<InvoiceSettings> GetSettingAsync();
        Task<InvoiceSettings> AddSettingAsync(InvoiceSettings settings);
        Task UpdateSettingAsync(InvoiceSettings settings);
    } // end i
} // end ns
