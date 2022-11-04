using System;

namespace NorthWindCoreLibrary.Models
{
    public class DataContainer
    {
        public int OrderId { get; }
        public string CompanyName { get; }
        public string FirstName { get; }
        public string ProductName { get; }
        public string CategoryName { get; }
        public DateTime? OrderDate { get; }
        public decimal? Freight { get; }

        public DataContainer(
            int orderId, 
            string companyName, 
            string firstName, 
            string productName, 
            string categoryName, 
            DateTime? orderDate, 
            decimal? freight)
        {
            OrderId = orderId;
            CompanyName = companyName;
            FirstName = firstName;
            ProductName = productName;
            CategoryName = categoryName;
            OrderDate = orderDate;
            Freight = freight;
        }
    }
}
