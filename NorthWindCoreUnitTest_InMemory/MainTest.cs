using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCoreExtensions;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthWindCoreLibrary.Classes;
using NorthWindCoreLibrary.Classes.Helpers;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.LanguageExtensions;
using NorthWindCoreLibrary.Models;
using NorthWindCoreUnitTest_InMemory.Base;
using NorthWindCoreUnitTest_InMemory.DataProvider;
using NorthWindCoreUnitTest_InMemory.ValidationClasses;
using Customers = NorthWindCoreLibrary.Models.Customers;

namespace NorthWindCoreUnitTest_InMemory
{
    /// <summary>
    /// Basic to intermediate code samples for working with EF Core 5 where data comes from several json files
    /// under the unit test executable, Json folder
    ///
    /// DO NOT alter the Json files under the executable path unless you understand the consequences which
    /// are multiple test methods rely on the data in these json files.
    ///
    /// Karen notes:
    ///     Talk about HasQueryFilter/IgnoreQueryFilters
    ///     ToQueryString not working for in-memory testing
    ///
    /// If a developer does not have issues connecting to the NorthWind2020 database
    /// a few steps will allow you to run ignored test methods
    /// </summary>
    [TestClass]
    public partial class MainTest : TestBase
    {


        [TestMethod]
        [TestTraits(Trait.Warming)]
        //[Ignore]
        public void A_Warmup()
        {
            ContactOperations.Warmup();
        }

        /// <summary>
        /// Read all customers into a list of <see cref="Customers"/>
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWork)]
        public void CustomerReadAll()
        {
            // Discuss
            //var customers = Context.Customers.IgnoreQueryFilters().ToList();

            var customers = Context.Customers.ToList();
            Assert.AreEqual(customers.Count, 91);

        }

        [TestMethod]
        [TestTraits(Trait.OffBase)]
        [Ignore]
        public void Name1()
        {
            Customers customer = new() { CompanyName = "ABC" };

            using var context = new NorthwindContext();

            context.Entry(customer).State = EntityState.Added;

            context.SaveChanges();

        }
        [TestMethod]
        [TestTraits(Trait.OffBase)]
        [Ignore]
        public void Name2()
        {
            Customers customer = new() { CompanyName = "ABC" };

            using var context = new NorthwindContext();
            context.Entry(customer).State = EntityState.Added;
            context.Customers.Add(new Customers() { CompanyName = "DEF" });
            context.SaveChanges();
        }
        /// <summary>
        /// Get all customers from Mexico
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWork)]
        public void CustomerReadWhereCountryIsMexico()
        {

            var customers = Context
                .Customers
                .Where(customer => customer.CountryIdentifier == 12)
                .ToList();

            Assert.AreEqual(customers.Count, 6);

        }

        /// <summary>
        /// Get all customers from Mexico with contact type of Owner
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWork)]
        public void CustomerReadWhereCountryIsMexicoAndIsOwner()
        {

            List<Customers> customerList = Context.Customers
                .Where(currentCustomer =>
                    currentCustomer.CountryIdentifier == 12 && 
                    currentCustomer.ContactTypeIdentifier == 7)
                .ToList();

            Assert.AreEqual(customerList.Count, 4);

        }

        /// <summary>
        /// Read all <see cref="Contacts"/>
        ///     Validate record count (normally done using a data provider)
        ///     Validate all contacts have their contact type property populates
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWork)]
        public void ContactsReadAll()
        {
            var contacts = Context.Contacts.ToList();

            Assert.AreEqual(contacts.Count, 91);

  
            Assert.IsTrue(contacts.All(currentContact => 
                currentContact.ContactTypeIdentifierNavigation is not null));

        }

        /// <summary>
        /// Get all contacts of type
        ///     Owner 7
        ///     Sales Representative 12
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWork)]
        public void ContactsReadWhereIn()
        {

            List<int> identifiers = new() { 7, 12 };

            var contacts = Context.Contacts
                .Where(currentContact =>
                    currentContact.ContactTypeIdentifier.HasValue &&
                    identifiers.Contains(currentContact.ContactTypeIdentifier ?? 0)).ToList();

            Assert.AreEqual(contacts.Count, 33);

        }

        /// <summary>
        /// Mockup for adding a single <see cref="Customers"/>
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWorkCrud)]
        public void AddSingleNewCustomer()
        {


            Context.Entry(SingleContact).State = EntityState.Added;

            var customer = new Customers()
            {
                CompanyName = "Karen's coffee shop",
                Contact = SingleContact,
                CountryIdentifier = 20,
                CountryIdentifierNavigation = new Countries() { Name = "USA" }
            };

            Context.Entry(customer).State = EntityState.Added;

            var saveChangesCount = Context.SaveChanges();

            Assert.IsTrue(saveChangesCount == 2,
                "Expect one customer and one contact to be added.");

        }

        [TestMethod]
        [TestTraits(Trait.StudentWorkCrud)]
        public void CustomersAddRange()
        {

            using var context = new NorthwindContext(dbContextRemoveOptions);

            context.Customers.AddRange(MockedInMemoryCustomers());
            context.Contacts.AddRange(MockedInMemoryContacts());

            context.SaveChanges();

            Assert.IsTrue(
                context.Customers.Count() == 20 &&
                context.Customers.ToList().All(currentCustomer => currentCustomer.Contact is not null)
            );

            var someCustomers = context.Customers.Take(3).ToList();

            context.Customers.RemoveRange(someCustomers);
            context.SaveChanges();

            Assert.AreEqual(context.Customers.Count(), 17);


        }
        /// <summary>
        /// There are 91 customers, remove a range of customers and validate
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWorkCrud)]
        public void CustomersRemoveRange()
        {

            var someCustomers = Context.Customers.Take(3).ToList();

            Context.Customers.RemoveRange(someCustomers);
            Context.SaveChanges();

            Assert.AreEqual(Context.Customers.Count(), 88);

        }

        /// <summary>
        /// Before EF Core 5 there was no method to filter related data, now we can use
        ///     .Where, .OrderBy, .OrderByDescending and .Take(x)
        /// </summary>
        /// <remarks>
        /// See
        ///
        /// Include uses Eager loading for related data
        /// https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager
        /// 
        /// Filtered include
        /// https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager#filtered-include
        /// 
        /// </remarks>
        [TestMethod]
        [TestTraits(Trait.Filtering)]
        //[Ignore]
        public void FilteredInclude()
        {

            var germanyCountryIdentifier = 9;

            using var data = new NorthwindContext();

            var customersList = data.Customers.AsNoTracking()
                .Include(customer => customer.Orders)
                    .Where(currentCustomer => currentCustomer.CountryIdentifier == germanyCountryIdentifier)
                .ToList();

            Assert.IsTrue(customersList.Count == 11);

        }


        /// <summary>
        /// Test we can return a single customer details including navigation properties
        /// Each test has hard coded values while the original test asserted against the
        /// database tables using a data provider which can not be used as at this time
        /// you don't have SQL-Server access
        ///
        /// See <see cref="SqlOperations.GetCustomers"/> for how the test would run with
        /// database access
        /// 
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Relations)]
        public void LoadingRelations()
        {
            int customerIdentifier = 3;

            var singleCustomer = Context.Customers
                .IncludeContactsDevicesCountry()
                .FirstOrDefault(customer => customer.CustomerIdentifier == customerIdentifier);



            /*
             * Null-conditional operators ?. and ?[]
             * https://docs.fluentvalidation.net/en/latest/built-in-validators.html
             */

            Assert.AreEqual(singleCustomer?.CompanyName, "Antonio Moreno Taquería");
            Assert.AreEqual(singleCustomer?.CountryIdentifierNavigation.Name, "Mexico");
            Assert.AreEqual(singleCustomer?.Contact.FirstName, "Antonio");
            Assert.AreEqual(singleCustomer?.Contact.LastName, "Moreno");
            Assert.IsTrue(singleCustomer?.Contact.ContactDevices.FirstOrDefault().PhoneTypeIdentifierNavigation.PhoneTypeDescription == "Office");
            Assert.AreEqual(singleCustomer.Contact.ContactDevices.FirstOrDefault()?.PhoneNumber, "(171) 555-7788");


        }

        /// <summary>
        /// Office and home are the same
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.Relations)]
        public void LoadingTheSinkRelations()
        {
            int customerIdentifier = 1;

            var singleCustomer = Context.Customers
                .IncludeContactsDevicesCountry()
                .FirstOrDefault(customer => customer.CustomerIdentifier == customerIdentifier);


            Debug.WriteLine($"{singleCustomer.Contact.FirstName} {singleCustomer.Contact.LastName}");
            Debug.WriteLine(new string('_', 20));

            foreach (var device in singleCustomer.Contact.ContactDevices)
            {
                // ReSharper disable once PossibleInvalidOperationException
                Debug.WriteLine($"{GetPhoneType(device.PhoneTypeIdentifier.Value)} {device.Contact.LastName} {device.PhoneNumber}");
                Debug.WriteLine("");
            }

            List<string> expected = new() { "(5) 555-4729", "(5) 555-4729", "456-987-1234" };

            var phones = singleCustomer.Contact.ContactDevices.Select(x => x.PhoneNumber).ToList();

            CollectionAssert.AreEqual(expected, phones);

        }

        [TestMethod]
        [TestTraits(Trait.EntityFrameworkExtensions)]
        public void GetModelNamesTest()
        {

            List<string> modelNamesExpected = new()
            {
                "BusinessEntityPhone", "Categories", "ContactDevices", "Contacts", "ContactType", "Countries",
                "Customers", "Employees", "EmployeeTerritories", "OrderDetails", "Orders", "PhoneType",
                "Products", "Region", "Shippers", "Suppliers", "Territories"
            };

            List<string> modelNames = Context.GetModelNames().OrderBy(x => x).ToList();

            CollectionAssert.AreEqual(modelNames, modelNamesExpected);

            // must have permissions to read SQL-Server
            //CollectionAssert.AreEqual(modelNames,SqlOperations.TableNames());

        }

        [TestMethod]
        [TestTraits(Trait.EntityFrameworkExtensions)]
        public void GetColumnNamesForModelTest()
        {

            List<string> expectedColumnList = new ()
            {
                "City", "CompanyName", "ContactId", "ContactTypeIdentifier", "CountryIdentifier", "CustomerIdentifier", "Fax",
                "ModifiedDate", "Phone", "PostalCode", "Region", "Street"
            };

            /*
             * Work against Customers model
             */
            var modelName = Context.GetModelNames().FirstOrDefault(tableName => tableName == nameof(Customers));

            /*
             * Column names for model using ColumnNames DbContext extension method
             */
            var columnNames = Context.ColumnNames(modelName).OrderBy(column => column).ToList();
            CollectionAssert.AreEqual(columnNames, expectedColumnList);

            /*
             * Column names from database to verify column names from ColumnNames DbContext extension method
             * NOTE: Must have permissions to read from SQL-Server
             */
            var columns = SqlOperations.ColumnNamesForTable(modelName);
            CollectionAssert.AreEqual(columns, expectedColumnList);

        }


        /// <summary>
        /// Demonstrates sort by property name as a string.
        /// 
        /// Why?
        /// Suppose we had a control populated with property names, this means
        /// we need to have a way to query EF where conventionally we hard code
        /// the property name. This solves this problem.
        /// 
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.CustomSorting)]
        public void CustomerCustomSort_City()
        {

            List<Customers> customersList = Context.Customers
                .Include(customer => customer.CountryIdentifierNavigation)
                .Include(customer => customer.Contact)
                .ThenInclude(currentContact => currentContact.ContactDevices)
                .ThenInclude(devices => devices.PhoneTypeIdentifierNavigation)
                .ToList()
                .SortByPropertyName("CompanyName", SortDirection.Descending);

            Assert.IsTrue(customersList.FirstOrDefault().City == "Warszawa");
            Assert.IsTrue(customersList.LastOrDefault().City == "Berlin");

            StringBuilder builder = new();

            foreach (var item in customersList)
            {
                builder.AppendLine($"{item.CompanyName,-45}{item.City}");
            }

            File.WriteAllText(nameof(CustomerCustomSort_City) + ".txt", builder.ToString());

        }

        /// <summary>
        /// Demonstrates obtaining the query generated by EF Core using non in-memory DbContext
        /// as <seealso cref="EntityFrameworkQueryableExtensions.ToQueryString"/> does not support This extension
        ///
        /// Entity Framework Core can reveal a query by setting up logging in a DbContext which means each LINQ
        /// statement will write it's query to the desired output e.g. log file (normally for production) or
        /// to the console (usually for debugging).
        ///
        /// So ToQueryString is something to use one a LINQ statement is not providing proper results which can
        /// happen with improper joins and/or a bad database design.
        ///
        /// Notes
        ///  - ToQueryString works without actually making a call to a database
        ///  - ToQueryString is new, there may be some spots where it does not work as intend
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.StudentWorkUtility)]
        public void GetQueryString()
        {
            using var context = new NorthwindContext();
            var query = context.Customers
                .Include(customer => customer.CountryIdentifierNavigation)
                .Include(customer => customer.Contact)
                .ThenInclude(currentContact => currentContact.ContactDevices)
                .ThenInclude(devices => devices.PhoneTypeIdentifierNavigation).ToQueryString();

            Debug.WriteLine(query);

        }

        [TestMethod]
        [TestTraits(Trait.CRUD)]
        public void RemoveSingleCustomer()
        {

            Assert.IsTrue(DeleteCustomer());
        }

        /// <summary>
        /// Find by primary key
        /// Finds an entity with the given primary key value
        /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext.find?view=efcore-5.0
        ///
        /// This is more efficient than using FirstOrDefault which is generic accepting a predicate while the Find
        /// method works with a indexed primary key which first checks cached data and if not found queries the database table
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.AccessTrackedEntities)]
        public void FindByPrimaryKey()
        {
            var customer = Context.Customers.Find(3);
            Assert.IsTrue(customer.CompanyName == "Antonio Moreno Taquería");
        }

        /// <summary>
        /// Example for obtaining current and original values of a tracked entity
        ///
        /// Note CurrentValue and OriginalValue are get/setters
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.AccessTrackedEntities)]
        public void ContactOriginalCurrentValueCheck()
        {
            var firstName = "Karen";
            var changedFirstName = "Mary";

            // create a new Contact and save
            Contacts contacts = new ()
            {
                FirstName = firstName
            };

            Contacts contact1 = new()
            {
                ContactId = 1,
                FirstName = "Bick",
                LastName = "VU"
            };


            Context.Add(contacts);
            Assert.IsTrue(Context.SaveChanges() == 1);

            // get current first name
            var currentFirstName = Context.Entry(SingleContact)
                .Property(currentContact => currentContact.FirstName).CurrentValue;

            Assert.AreEqual(currentFirstName, firstName);

            // change first name
            contacts.FirstName = changedFirstName;

            // validate first name changed
            Assert.AreEqual(contacts.FirstName, changedFirstName);

            // get original first name
            var originalFirstName = Context.Entry(SingleContact)
                .Property(currentContact => currentContact.FirstName).OriginalValue;

            // assert we got the original value
            Assert.IsTrue(originalFirstName == firstName);


            /*
             * Let's clone the  contact (without as Contacts clonedContact is type object but we know better)
             */
            var clonedContact = Context.Entry(contacts).GetDatabaseValues().ToObject() as Contacts;

            /*
             * In short set all properties of contact1 to contact object
             */
            Context.Entry(contacts).CurrentValues.SetValues(contact1);
            Assert.IsTrue(contacts.LastName == "VU");

            /*
             * Assert the clone contact last name is empty
             */
            Assert.IsNull(clonedContact.LastName);
        }


        #region Working with live data, same can be done with in-memory


        [TestMethod]
        [TestTraits(Trait.AccessTrackedEntities)]
        //[Ignore]
        public void FindAndLoadSingleCollection()
        {
            using var context = new NorthwindContext();

            var customer = context.Customers.Find(3);
            Assert.IsTrue(customer.CompanyName == "Antonio Moreno Taquería");

            Assert.IsTrue(customer.Orders.Count == 0);
            context.Entry(customer).Collection(e => e.Orders).Load();
            Assert.IsTrue(customer.Orders.Count > 0);

        }

        /// <summary>
        /// Demonstrates modifying a entry state
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.AccessTrackedEntities)]
        //[Ignore]
        public void FindAndModifySingleEntry()
        {
            using var context = new NorthwindContext();

            var customer = context.Customers.Find(3);
            Assert.IsTrue(customer.CompanyName == "Antonio Moreno Taquería");
            Assert.IsTrue(context.Entry(customer).State == EntityState.Unchanged);

            customer.CompanyName = "ABC";
            Assert.IsTrue(context.Entry(customer).State == EntityState.Modified);
        }

        /// <summary>
        /// Example for a like condition for starts with and case insensitive.
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.LikeConditions)]
        public void LikeStartsWith()
        {
            Context.Customers.AddRange(MockedInMemoryCustomers());
            Context.SaveChanges();

            List<Customers> results = Context.Customers
                .Where(customer => EF.Functions.Like(customer.CompanyName, "an%"))
                .ToList();


            Assert.AreEqual(results.Count, 2);
            
        }

        [TestMethod]
        [TestTraits(Trait.LikeConditions)]
        public void LikeEndWith()
        {
            Context.Customers.AddRange(MockedInMemoryCustomers());
            Context.SaveChanges();

            List<Customers> customersList = Context.Customers
                .Where(customer => EF.Functions.Like(customer.CompanyName, "%S.A."))
                .ToList();

            Assert.AreEqual(customersList.Count, 1);
        }

        [TestMethod]
        [TestTraits(Trait.LikeConditions)]
        public void LikeContains()
        {
            Context.Customers.AddRange(MockedInMemoryCustomers());
            Context.SaveChanges();

            List<Customers> results = Context.Customers
                .Where(customer => EF.Functions.Like(customer.CompanyName, "%Comidas%"))
                .ToList();

            Assert.AreEqual(results.Count, 2);


        }

        /// <summary>
        /// Change a date property value
        /// </summary>
        /// <remarks>
        /// Side note, we can work with a modified date/time using shadow properties
        /// https://social.technet.microsoft.com/wiki/contents/articles/53662.entity-framework-core-shadow-properties-c.aspx
        /// </remarks>
        [TestMethod]
        [TestTraits(Trait.AccessTrackedEntities)]
        //[Ignore]
        public void ChangeCurrentValueByType()
        {
            var expectedDate = new DateTime(2021, 7, 4);
            using var context = new NorthwindContext();

            /*
             * Get Customer by primary key
             */
            var customer = context.Customers.Find(3);

            /*
             * Rather than directly setting ModifiedDate we look for it via type
             * Note: DateTime will fail as ModifiedDate is nullable
             */
            foreach (var propertyEntry in context.Entry(customer).Properties)
            {

                if (propertyEntry.Metadata.ClrType == typeof(DateTime?))
                {
                    propertyEntry.CurrentValue = expectedDate;
                }
            }

            // Assert
            Assert.AreEqual(customer.ModifiedDate, expectedDate);


            /*
             * Get original values from the database
             */
            var originalCustomer = context.Customers.AsNoTracking()
                .FirstOrDefault(cust => cust.CustomerIdentifier == customer.CustomerIdentifier);

            /*
             * Revert to ModifiedDate original value
             */
            customer.ModifiedDate = originalCustomer.ModifiedDate;

            // Assert
            Assert.AreNotEqual(customer.ModifiedDate, expectedDate);


        }

        #endregion

        #region fluent validation https://docs.fluentvalidation.net/en/latest/built-in-validators.html

        /// <summary>
        /// No Assert required, on failure an exception is thrown
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.FluentValidation)]
        public void ValidateCompanyNameIsNull()
        {

            var singleCustomers = new Customers() { CompanyName = null };
            TestValidationResult<Customers> result = customersValidator.TestValidate(singleCustomers);
            result.ShouldHaveValidationErrorFor(customer => customer.CompanyName);
            result.ShouldHaveValidationErrorFor(customer => customer.ModifiedDate);

        }

        /// <summary>
        /// Inspect violations
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.FluentValidation)]
        public void ValidateCompanyNameIsNull_1()
        {

            var singleCustomers = new Customers() { CompanyName = null };


            ValidationResult results = customersValidator.Validate(singleCustomers);

            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    Console.WriteLine($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}");
                }
            }

        }

        /// <summary>
        /// No Assert required, on failure an exception is thrown
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.FluentValidation)]
        public void ValidateCompanyNameIsNotNull()
        {

            var singleCustomer = MockedInMemoryCustomers().FirstOrDefault();

            TestValidationResult<Customers> result = customersValidator1.TestValidate(singleCustomer);

            result.ShouldNotHaveAnyValidationErrors();

        }

        #endregion

        /// <summary>
        /// Out of place code sample
        /// 
        /// Written to answer a forum question which was accepted.
        ///
        ///10/21/2021 Karen changed code to read from json file rather than SQL-Server
        /// 
        /// </summary>
        [TestMethod]
        //[Ignore]
        public void LargeLike()
        {
            var (contracts, exception) = SqlOperations1.ReadJsonView("%nia");

            if (contracts.Count > 0 && exception is null)
            {
                Debug.WriteLine(contracts.Count);
            }
            else
            {
                Debug.WriteLine(exception is not null ? exception.Message : "No matches");
            }

            Assert.AreEqual(contracts.Count, 4704);
        }

    }
}
