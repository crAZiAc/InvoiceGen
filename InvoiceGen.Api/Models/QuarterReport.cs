using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api.Models
{
    public class QuarterReport
    {
        public string Quarter { get; set; }
        public double TotalAmount { get; set; }
        public double TotalAmountWithVat { get; set; }
        public double TotalVatAmount { get; set; }
        public int NumberOfInvoices { get; set; }
    } // end c
} // end ns
