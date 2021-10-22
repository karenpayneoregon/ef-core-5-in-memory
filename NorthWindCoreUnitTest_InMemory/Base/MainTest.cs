using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NorthWindCoreLibrary.Classes.Helpers;
using NorthWindCoreLibrary.Classes.North.Classes;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;
using NorthWindCoreLibrary.Projections;
using NorthWindCoreUnitTest_InMemory.ValidationClasses;


// ReSharper disable once CheckNamespace - do not change
namespace NorthWindCoreUnitTest_InMemory
{
    public partial class MainTest
    {
        /// <summary>
        /// Options for in-memory testing
        /// </summary>
        readonly DbContextOptions<NorthwindContext> dbContextStandardOptions = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        /// <summary>
        /// Delete entity requires different options than other operations
        /// </summary>
        readonly DbContextOptions<NorthwindContext> dbContextRemoveOptions =  new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(databaseName: "Remove_Customer_to_database")
            .Options;

        #region Fluent Validator rules

        private CustomersValidator customersValidator;
        private CustomersValidator1 customersValidator1;

        #endregion

        /// <summary>
        /// Single instance of the <see cref="NorthwindContext"/> for in-memory context
        /// </summary>
        private NorthwindContext Context;

        /// <summary>
        /// Perform initialization before test runs using assertion on current test name.
        /// </summary>
        [TestInitialize]
        public void Initialization()
        {
            Context = new NorthwindContext(dbContextStandardOptions);

            annihilationList = new List<object>();

            if (TestContext.TestName == nameof(LoadingRelations) || 
                TestContext.TestName == nameof(LoadingTheSinkRelations) ||
                TestContext.TestName == nameof(FindByPrimaryKey) ||
                TestContext.TestName == nameof(CustomerCustomSort_City) ||
                TestContext.TestName == nameof(CustomersRemoveRange) || 
                TestContext.TestName == nameof(CustomerReadAll) ||
                TestContext.TestName == nameof(CustomerReadWhereCountryIsMexico) ||
                TestContext.TestName == nameof(CustomerReadWhereCountryIsMexicoAndIsOwner) ||
                TestContext.TestName == nameof(ContactsReadAll) ||
                TestContext.TestName == nameof(ContactsReadWhereIn) ||
                TestContext.TestName == nameof(GetQueryString)) { LoadJoinedData(); }

            if (TestContext.TestName == nameof(FilteredInclude))
            {
                
                Context.Customers.AddRange(JsonConvert.DeserializeObject<List<Customers>>(File.ReadAllText(customersJsonFileName))!);
                Context.Orders.AddRange(JsonConvert.DeserializeObject<List<Orders>>(File.ReadAllText(ordersJsonFileName))!);

                Context.SaveChanges();
            }
            
            if (TestContext.TestName == nameof(ValidateCompanyNameIsNull) ||
                TestContext.TestName == nameof(ValidateCompanyNameIsNull_1)) { customersValidator = new CustomersValidator(); }

            if (TestContext.TestName == nameof(ValidateCompanyNameIsNotNull))
            {
                customersValidator1 = new CustomersValidator1();
            }
        }

        /// <summary>
        /// Perform cleanup after test runs using assertion on current test name.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.TestName == nameof(CreateJsonFilesTask))
            {
                // Lisa - demo TestCleanup
            }
        }
        /// <summary>
        /// Perform any initialize for the class
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestResults = new List<TestContext>();
        }

        /// <summary>
        /// Base instance of a <see cref="Contacts"/>
        /// </summary>
        public Contacts SingleContact => new()
        {
            FirstName = "Karen",
            LastName = "Payne",
            ContactTypeIdentifier = 1,
            ContactTypeIdentifierNavigation = new ContactType()
            {
                ContactTypeIdentifier = 1,
                ContactTitle = "Accounting Manager",
                Contacts = new List<Contacts>() { new()
                {
                    ContactId = 1, 
                    ContactTypeIdentifier = 1,
                    FirstName = "Bernardo",
                    LastName = "Batista"
                } },
            },
            ContactDevices = new List<ContactDevices>()
        };
        
        

        #region Json files generation

        private static readonly string customersJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Customers.json");
        private static readonly string contactTypeJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "ContactType.json");
        private static readonly string contactsJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Contacts.json");
        private static readonly string contactsJsonFileName1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Contacts1.json");
        private static readonly string countriesJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Countries.json");
        private static readonly string contactDevicesJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "ContactDevices.json");
        
        private static readonly string ordersJsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json", "Orders.json");

        public static async Task SerializeContactsAlternateToJson()
        {
            await using var context = new NorthwindContext();
            List<Contacts> contacts = context.Contacts.ToList();
            await File.WriteAllTextAsync(contactsJsonFileName1, JsonHelpers.Serialize<Contacts>(contacts));
        }
        public static async Task SerializeOrdersToJson()
        {
            await using var context = new NorthwindContext();
            List<Orders> orders = context.Orders.ToList();
            await File.WriteAllTextAsync(ordersJsonFileName, JsonHelpers.Serialize<Orders>(orders));
        }

        public static async Task SerializeModelsToJson()
        {

            await using var context = new NorthwindContext();

            List<CustomerEntity> cust = await AllCustomersToJsonAsync();
            await File.WriteAllTextAsync(customersJsonFileName, JsonHelpers.Serialize<CustomerEntity>(cust));

            List<ContactType> contactTypes = context.ContactType.ToList();
            await File.WriteAllTextAsync(contactTypeJsonFileName, JsonHelpers.Serialize<ContactType>(contactTypes));

            List<Contacts> contacts = context.Contacts.ToList();
            await File.WriteAllTextAsync(contactsJsonFileName, JsonHelpers.Serialize<Contacts>(contacts));

            List<Countries> countriesList = context.Countries.ToList();
            await File.WriteAllTextAsync(countriesJsonFileName, JsonHelpers.Serialize<Countries>(countriesList));

            try
            {
                List<ContactDevices> contactDeviceList = context.ContactDevices.ToList();
                await File.WriteAllTextAsync(contactDevicesJsonFileName, JsonHelpers.Serialize<ContactDevices>(contactDeviceList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        
        public static ContactItem contact => new ()
        {
            ContactId = 1, 
            ContactTitle = "Accounting Manager", 
            FirstName = "Maria", 
            LastName = "Anders", 
            ContactTypeIdentifier = 1
        };

        public static async Task<List<CustomerEntity>> AllCustomersToJsonAsync()
        {

            return await Task.Run(async () =>
            {
                await using var context = new NorthwindContext();
                List<CustomerEntity> customerItemsList = await context.Customers
                    .Include(customer => customer.Contact)
                    .Select(Customers.Projection)
                    .ToListAsync();

                return customerItemsList.OrderBy((customer) => customer.CompanyName).ToList();
            });

        }
        
        private static List<Customers> CustomersRelationsData(
            out List<ContactType> contactTypeList, 
            out List<Contacts> contactList, 
            out List<Countries> countriesList, 
            out List<ContactDevices> contactDevicesList,
            out List<PhoneType> phoneTypesList)
        {
            List<Customers> customersList = JsonConvert.DeserializeObject<List<Customers>>(File.ReadAllText(customersJsonFileName));
            
            contactTypeList = JsonConvert.DeserializeObject<List<ContactType>>(File.ReadAllText(contactTypeJsonFileName));
            contactList = JsonConvert.DeserializeObject<List<Contacts>>(File.ReadAllText(contactsJsonFileName));
            countriesList = JsonConvert.DeserializeObject<List<Countries>>(File.ReadAllText(countriesJsonFileName));
            contactDevicesList = JsonConvert.DeserializeObject<List<ContactDevices>>(File.ReadAllText(contactDevicesJsonFileName));

            phoneTypesList = new List<PhoneType>()
            {
                new () {PhoneTypeDescription = "Home"},
                new () {PhoneTypeDescription = "Cell"},
                new () {PhoneTypeDescription = "Office"}
            };

            return customersList;
            
        }

        public void LoadJoinedData()
        {
            Context = new NorthwindContext(dbContextStandardOptions);
            
            var customersList = CustomersRelationsData(
                out var contactTypeList, 
                out var contactList, 
                out var countriesList, 
                out var contactDevicesList, 
                out var phoneTypesList);

            Context.Customers.AddRange(customersList!);
            Context.ContactType.AddRange(contactTypeList!);
            Context.Contacts.AddRange(contactList!);
            Context.Countries.AddRange(countriesList!);
            Context.ContactDevices.AddRange(contactDevicesList);
            Context.PhoneType.AddRange(phoneTypesList);
            
            var count = Context.SaveChanges();
        }

        #endregion

        #region Annihilation

        protected List<object> annihilationList;

        protected T AddSandboxEntity<T>(T sandboxEntity) where T : class
        {
            annihilationList.Add(sandboxEntity);

            return sandboxEntity;
        }
        protected IEnumerable<T> GetSandboxEntities<T>() => (annihilationList.Where(item => item.GetType() == typeof(T)).Select(item => (T)item));

        #endregion

        #region Code responsible for obtaining mocked data 

        /// <summary>
        /// Mocked up customers for various in-memory test
        /// </summary>
        /// <returns></returns>
        public static List<Customers> MockedInMemoryCustomers()
        {
            var customers = new List<Customers>()
            {
                new () {CompanyName = "Ana Trujillo Emparedados y helados", ContactId = 2, ContactTypeIdentifier = 7,CountryIdentifier = 12},
                new () {CompanyName = "Antonio Moreno Taquería", ContactId = 3, ContactTypeIdentifier = 7,CountryIdentifier = 12},
                new () {CompanyName = "Around the Horn", ContactId = 4, ContactTypeIdentifier = 12, CountryIdentifier = 19},
                new () {CompanyName = "Berglunds snabbköp", ContactId = 5, ContactTypeIdentifier = 6, CountryIdentifier = 17},
                new () {CompanyName = "Blauer See Delikatessen", ContactId = 6, ContactTypeIdentifier = 12,CountryIdentifier = 9},
                new () {CompanyName = "Blondesddsl père et fils", ContactId = 7, ContactTypeIdentifier = 5,CountryIdentifier = 8},
                new () {CompanyName = "Bólido Comidas preparadia", ContactId = 8, ContactTypeIdentifier = 7,CountryIdentifier = 16},
                new () {CompanyName = "Cactus Comidas para llevar", ContactId = 11, ContactTypeIdentifier = 9,CountryIdentifier = 1},
                new () {CompanyName = "Consolidated Holdings", ContactId = 14, ContactTypeIdentifier = 12,CountryIdentifier = 19},
                new () {CompanyName = "Drachenblut Delikatessen", ContactId = 15, ContactTypeIdentifier = 6,CountryIdentifier = 9},
                new () {CompanyName = "Du monde entier", ContactId = 16, ContactTypeIdentifier = 7, CountryIdentifier = 8},
                new () {CompanyName = "Eastern Connection", ContactId = 17, ContactTypeIdentifier = 9, CountryIdentifier = 19},
                new () {CompanyName = "Ernst Handel", ContactId = 18, ContactTypeIdentifier = 11, CountryIdentifier = 2},
                new () {CompanyName = "FISSA Fabrica Inter. Salchichas S.A.", ContactId = 15, ContactTypeIdentifier = 1,CountryIdentifier = 16},
                new () {CompanyName = "Folies gourmandes", ContactId = 20, ContactTypeIdentifier = 2, CountryIdentifier = 8},
                new () {CompanyName = "Folk och fä HB", ContactId = 21, ContactTypeIdentifier = 7, CountryIdentifier = 17},
                new () {CompanyName = "Frankenversand", ContactId = 22, ContactTypeIdentifier = 5, CountryIdentifier = 9},
                new () {CompanyName = "France restauration", ContactId = 23, ContactTypeIdentifier = 5, CountryIdentifier = 8},
                new () {CompanyName = "Franchi S.p.A.", ContactId = 24, ContactTypeIdentifier = 12, CountryIdentifier = 11},
                new () {CompanyName = "Furia Bacalhau e Frutos do Mar", ContactId = 25, ContactTypeIdentifier = 11,CountryIdentifier = 15}
            };

            return customers;

        }
        /// <summary>
        /// Setup in memory contacts.
        /// Note context.Contact.Clear(), without this had some
        /// strange things happen, will look deeper into this.
        /// </summary>
        /// <returns></returns>
        public List<Contacts> MockedInMemoryContacts()
        {
            using var context = new NorthwindContext(dbContextStandardOptions);

            context.Database.EnsureDeleted();
            context.Contacts.AddRange(MockedContacts());
            context.SaveChanges();

            return context.Contacts.ToList();

        }
        /// <summary>
        /// Read contact data from json file into List&lt;Contact&gt;
        /// </summary>
        /// <returns></returns>
        protected List<Contacts> MockedContacts() => 
            JsonConvert.DeserializeObject<List<Contacts>>(File.ReadAllText(contactsJsonFileName));

        #endregion


        /// <summary>
        /// Remove customer, mark contact as not in use
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// All mockups must be saved before performing
        /// the customer remove operation.
        /// </remarks>
        public bool DeleteCustomer()
        {
            var companyName = "Around the Horn";

            try
            {
                using var context = new NorthwindContext(dbContextRemoveOptions);
                
                /*
                * Mock-up tables
                */
                context.Customers.AddRange(MockedInMemoryCustomers());

                var test1 = context.Contacts.ToList();

                //context.Contacts.AddRange(MockedInMemoryContacts());
                context.SaveChanges();

                /*
                * Find customer and contact
                */
                var customer = context.Customers.FirstOrDefault(
                    cust => cust.CompanyName == companyName);


                var contactList = context.Contacts.Where(x => x.ContactId < 20).ToList();
                var contact = context.Contacts.FirstOrDefault(
                    con => con.ContactId == customer.ContactId);

                if (contact is not null)
                {

                    /*
                     * Indicate state of entity to change tracker
                     */
                    context.Entry(customer!).State = EntityState.Modified;

                    /*
                     * Commit to in memory database
                     */
                    context.SaveChanges();

                    /*
                     * Remove the customer
                     */
                    context.Customers.Remove(customer);
                    
                    /*
                     * Commit to in memory database
                     */
                    context.SaveChanges();

                }

                /*
                 * The assertion
                 */
                return context.Customers.FirstOrDefault(cust => cust.CompanyName == companyName) is null;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get phone type description by phone type identifier.
        /// Note this is better done with a generic repository
        /// </summary>
        /// <param name="identifier">Phone type identifier</param>
        /// <returns>Phone description</returns>
        public string GetPhoneType(int identifier)
        {
            return Context.PhoneType.FirstOrDefault(pType => pType.PhoneTypeIdenitfier == identifier).PhoneTypeDescription;
        }
    }
}
