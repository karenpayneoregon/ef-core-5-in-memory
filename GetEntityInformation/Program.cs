using System;
using System.Collections.Generic;
using EntityCoreExtensions;
using Microsoft.EntityFrameworkCore;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.LanguageExtensions;
using NorthWindCoreLibrary.Models;

namespace GetEntityInformation
{
    partial class Program
    {
        static void Main(string[] args)
        {
            using var context = new NorthwindContext();
            Console.WriteLine(context.GetTableNameWithScheme<Customers>());

            List<Type> items = context.ModelTypeInformation();

            foreach (Type item in items)
            {
                //Console.WriteLine(context.GetTableNameBasic(item));
            }





            Console.ReadLine();
        }
    }
}
