using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NorthWindCoreLibrary.Models
{
    public partial class Customers
    {
        public override string ToString()
        {
            return $"{CustomerIdentifier} - {CompanyName}";
        }
    }
}
