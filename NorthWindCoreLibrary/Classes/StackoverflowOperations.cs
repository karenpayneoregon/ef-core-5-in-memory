using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;

namespace NorthWindCoreLibrary.Classes
{
    public class StackoverflowOperations
    {
        public static List<DataContainer> ReadData()
        {
            using (var context = new NorthwindContext())
            {
                return (from p in context.Products
                    join od in context.OrderDetails on p.ProductId equals od.ProductId
                    join c in context.Categories on p.CategoryId equals c.CategoryId
                    join o in context.Orders on od.OrderId equals o.OrderId
                    join e in context.Employees on o.EmployeeId equals e.EmployeeId
                    join cu in context.Customers on o.CustomerIdentifier equals cu.CustomerIdentifier
                    join s in context.Suppliers on p.SupplierId equals s.SupplierId
                    select new DataContainer(o.OrderId, cu.CompanyName, e.FirstName, p.ProductName, c.CategoryName,
                        o.OrderDate, o.Freight))
                    .ToList();
            }
        }
    }
}
