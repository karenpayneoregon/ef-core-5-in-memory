﻿using FluentValidation;
using NorthWindCoreLibrary.Models;
using Customers = NorthWindCoreLibrary.Models.Customers;

namespace NorthWindCoreUnitTest_InMemory.ValidationClasses
{
    /// <summary>
    /// Validate CompanyName is not null
    /// </summary>
    public class CustomersValidator1 : AbstractValidator<Customers>
    {
        public CustomersValidator1()
        {
            RuleFor(customer => customer.CompanyName).NotNull();
        }
    }
}