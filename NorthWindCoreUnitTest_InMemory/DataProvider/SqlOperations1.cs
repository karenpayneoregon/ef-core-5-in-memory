using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NorthWindCoreUnitTest_InMemory.DataProvider
{
    public class SqlOperations1
    {

        /// <summary>
        /// Note for class this database only exists on Karen's laptop
        /// </summary>
        public static string ConnectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=PaginationExample;Integrated Security=True";


        public static (List<Contract>, Exception exception) ReadDBView(string firstNameValue)
        {
            List<Contract> contracts = new();

            // TOP is used as the table has 100,000 rows and not indexed for this statement
            var selectStatement = 
                "SELECT TOP 7000  Id, FirstName, LastName, Balance " + 
                "FROM dbo.LotsOfData " + 
                "WHERE FirstName LIKE @FirstNameLike;";

            try
            {
                using var cn = new SqlConnection() { ConnectionString = ConnectionString };
                using var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement };

                cmd.Parameters.Add("@FirstNameLike", SqlDbType.NVarChar).Value = firstNameValue;
                
                cn.Open();

                var reader = cmd.ExecuteReader();

                if (!reader.HasRows) return (contracts, null);
                while (reader.Read())
                {
                    contracts.Add(new Contract()
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                            
                        Balance = reader.IsDBNull("Balance") ? 
                            (decimal?)null : 
                            reader.GetDecimal("Balance")
                                
                    });
                }

                return (contracts, null);

            }
            catch (Exception exception)
            {
                return (null, exception);
            }

            
        }
    }

    // place in own file
    public class Contract
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Balance { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";

    }
}
