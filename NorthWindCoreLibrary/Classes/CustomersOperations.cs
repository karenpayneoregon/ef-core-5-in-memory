using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthWindCoreLibrary.Classes.Helpers;
using NorthWindCoreLibrary.Classes.North.Classes;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;
using NorthWindCoreLibrary.Projections;


namespace NorthWindCoreLibrary.Classes
{
    public class CustomersOperations
    {
        public static async Task<List<CustomerItem>> GetCustomersWithProjectionAsync()
        {

            return await Task.Run(async () =>
            {
                await using var context = new NorthwindContext();
                return await context.Customers
                    .Select(CustomerItem.Projection)
                    .ToListAsync();
            });
        }

        
        #region Not part of part 4 lesson
        /// <summary>
        /// Custom projection for teaching sorting by property name as a string
        /// </summary>
        /// <returns>List&lt;<see cref="CustomerItemSort"/>&gt;</returns>
        public static async Task<List<CustomerItemSort>> GetCustomersWithProjectionSortAsync()
        {

            return await Task.Run(async () =>
            {
                await using var context = new NorthwindContext();
                return await context.Customers
                    .Select(CustomerItemSort.Projection)
                    .ToListAsync();
            });
        }

        #endregion

        public static CustomerEntity CustomerByIdentifier(int identifier)
        {
            using var context = new NorthwindContext();
            return context.Customers.Select(Customers.Projection)
                .FirstOrDefault(custEntity => custEntity.CustomerIdentifier == identifier);
        }

        public static async Task<List<Customers>> GetCustomersAsync()
        {

            return await Task.Run(async () =>
            {
                await using var context = new NorthwindContext();
                return await context.Customers
                    .ToListAsync();
            });
        }
    }
}
