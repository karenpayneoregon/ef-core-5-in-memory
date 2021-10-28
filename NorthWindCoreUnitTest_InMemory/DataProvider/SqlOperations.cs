using System;
using System.Collections.Generic;
using System.Data;
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


        /// <summary>
        /// Get table names a-z order excluding diagram table
        /// </summary>
        /// <returns></returns>
        public static List<string> TableNames()
        {
            var selectStatement = 
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " + 
                "WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME <> 'sysdiagrams' " + 
                "ORDER BY TABLE_NAME";

            using var cn = new SqlConnection() { ConnectionString = ConnectionString };
            using var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement };

            cn.Open();

            List<string> list = new();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }

            return list;

        }

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

        /// <summary>
        /// _ColumnName_ would be ColumnName or Position
        /// </summary>
        public static string ColumnDetailsForTable => 
            "SELECT col.ORDINAL_POSITION AS Position, col.COLUMN_NAME AS ColumnName, prop.value AS Description " +
            "FROM INFORMATION_SCHEMA.TABLES AS tbl INNER JOIN INFORMATION_SCHEMA.COLUMNS AS col ON col.TABLE_NAME = tbl.TABLE_NAME " + 
            "INNER JOIN sys.columns AS sc ON sc.object_id = OBJECT_ID(tbl.TABLE_SCHEMA + '.' + tbl.TABLE_NAME) AND sc.name = col.COLUMN_NAME " + 
            "LEFT OUTER JOIN sys.extended_properties AS prop ON prop.major_id = sc.object_id " + 
                "AND prop.minor_id = sc.column_id AND prop.name = 'MS_Description' " + 
            "WHERE tbl.TABLE_NAME = @TableName AND prop.value IS NOT NULL ORDER BY _ColumnName_";

        public static List<string> ColumnNamesForTable(string tableName, SortColumn sortColumn = SortColumn.ColumnName)
        {
            List<string> list = new();

            using var cn = new SqlConnection() { ConnectionString = ConnectionString };
            using var cmd = new SqlCommand()
            {
                Connection = cn, 
                CommandText = ColumnDetailsForTable.Replace("_ColumnName_", sortColumn.ToString())
            };

            cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = tableName;

            cn.Open();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(reader.GetString(1));
            }

            return list;
        }
    }

    public enum SortColumn
    {
        Position,
        ColumnName
    }
}
