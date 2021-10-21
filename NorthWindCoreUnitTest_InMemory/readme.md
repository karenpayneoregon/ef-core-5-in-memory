# About

![img](assets/BartalwaysTest.png)

Examples for `unit testing` Entity Framework Core 5 with `mocking` using `in memory` testing provider. 

:stop_sign: Using in memory testing is not a replacement for working against a live database.

:heavy_check_mark: Majority of test are mocked

:heavy_check_mark: Test classes are partial with reusable code in the base

:thumbsup: When adding a new test method once validated run all test methods to ensure no breakage

</br>
---

## NuGet packages

- [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/6.0.0-preview.6.21352.1) In-memory database provider for Entity Framework Core (to be used for testing purposes).
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) Json.NET is a popular high-performance JSON framework for .NET
  - [Documentation](https://www.newtonsoft.com/json)
- [FluentValidation](https://www.nuget.org/packages/FluentValidation/10.3.0?_src=template) A validation library for .NET that uses a fluent interface to construct strongly-typed validation rules.
  -  [Documentation](https://docs.fluentvalidation.net/en/latest/index.html)
 
# Build events

Open project properties, select Build Events, examine Post-build event command line.

```
if not exist $(TargetDir)\Json mkdir $(TargetDir)\Json
```

This ensures a folder named Json exists in the folder where binaries are located for placing Json files into for generating Json from models.

Since the files in the Json folder are under source control when cloning this solution they will be downloaded.


# Change tracking

## Accessing Tracked Entities

Microsoft docs: [There are four main APIs for accessing entities tracked by a DbContext](https://docs.microsoft.com/en-us/ef/core/change-tracking/entity-entries)

## Identity Resolution in EF Core

A DbContext can only track one entity instance with any given primary key value. This means multiple instances of an entity with the same key value must be resolved to a single instance. This is called "[identity resolution](https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution)". Identity resolution ensures Entity Framework Core (EF Core) is tracking a consistent graph with no ambiguities about the relationships or property values of the entities.

## Change Detection and Notifications

Each DbContext instance [tracks changes](https://docs.microsoft.com/en-us/ef/core/change-tracking/change-detection) made to entities.
