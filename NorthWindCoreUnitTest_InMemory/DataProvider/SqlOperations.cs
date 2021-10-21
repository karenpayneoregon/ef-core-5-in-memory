using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthWindCoreLibrary.Models;

namespace NorthWindCoreUnitTest_InMemory.DataProvider
{
    /// <summary>
    /// Provides code to validate EF Core lambda/LINQ queries
    ///
    /// For 10/22/2021 this code is not used
    /// </summary>
    public class SqlOperations
    {
        public static  string ConnectionString = 
            "Data Source=.\\SQLEXPRESS;Initial Catalog=NorthWind2020;Integrated Security=True";

        public static CustomerRelation GetCustomers(int identifier)
        {
            CustomerRelation customer = new ();

            /*
             * Query to match EF Core Lambda statement.
             * No need for a formal parameter as this is used for a unit test.
             */
            var selectStatement = File.ReadAllText(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                    "SQL_Queries", "SingleCustomerByCompanyName.sql"))
                .Replace("@CustomerIdentifier", identifier.ToString());

            using var cn = new SqlConnection() { ConnectionString = ConnectionString };
            using var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement };
            
            cn.Open();

            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                customer.CustomerIdentifier = reader.GetInt32(0);
                customer.CompanyName = reader.GetString(1);
                customer.City = reader.GetString(2);
                customer.PostalCode = reader.GetString(3);
                customer.ContactId = reader.GetInt32(4);
                customer.CountryIdentifier = reader.GetInt32(5);
                customer.Country = reader.GetString(6);
                customer.Phone = reader.GetString(7);
                customer.PhoneTypeIdentifier = reader.GetInt32(8);
                customer.ContactPhoneNumber = reader.GetString(9);
                customer.ModifiedDate = reader.GetDateTime(10);
                customer.FirstName = reader.GetString(11);
                customer.LastName = reader.GetString(12);
            }
            
            return customer;
            
        }
    }
}
