using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGen.Api
{
    public static class Extensions
    {
        public static int GetQuarter(this DateTime date)
        {
            return (date.Month + 2) / 3;
        }
    } // end c
} // end ns
