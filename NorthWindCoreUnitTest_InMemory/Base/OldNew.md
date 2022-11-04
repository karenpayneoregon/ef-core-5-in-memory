
```csharp
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
    TestContext.TestName == nameof(RemoveSingleCustomer) ||
    TestContext.TestName == nameof(GetQueryString)) 
{

    LoadJoinedData();

}
```

Alternate

```csharp
if (TestContext.TestName is nameof(LoadingRelations) or 
    nameof(LoadingTheSinkRelations) or 
    nameof(FindByPrimaryKey) or 
    nameof(CustomerCustomSort_City) or 
    nameof(CustomersRemoveRange) or 
    nameof(CustomerReadAll) or 
    nameof(CustomerReadWhereCountryIsMexico) or 
    nameof(CustomerReadWhereCountryIsMexicoAndIsOwner) or 
    nameof(ContactsReadAll) or 
    nameof(ContactsReadWhereIn) or 
    nameof(RemoveSingleCustomer) or 
    nameof(GetQueryString))
{

    LoadJoinedData();

}
```

