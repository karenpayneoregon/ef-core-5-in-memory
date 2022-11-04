using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NorthWindCoreLibrary.Models;
using Customers = NorthWindCoreLibrary.Models.Customers;

namespace NorthWindCoreUnitTest_InMemory.ValidationClasses
{
    /// <summary>
    /// Validate CompanyName is not null and Modified Date is not empty
    /// </summary>
    public class CustomersValidator : AbstractValidator<Customers>
    {
        public CustomersValidator()
        {
            RuleFor(customer => customer.CompanyName)
                .NotNull()
                .WithMessage("Dude we need a company name");
            
            RuleFor(customer => customer.ModifiedDate)
                .NotEmpty();
        }
    }
}
