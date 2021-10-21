using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindCoreLibrary.Classes
{
    /// <summary>
    /// For showing grouping
    /// </summary>
    public class CategoryProduct
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public override string ToString() => $"{CategoryName}, {ProductName}";

    }
}
