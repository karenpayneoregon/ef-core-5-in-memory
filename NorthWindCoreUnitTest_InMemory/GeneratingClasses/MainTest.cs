using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthWindCoreUnitTest_InMemory.Base;

// ReSharper disable once CheckNamespace
namespace NorthWindCoreUnitTest_InMemory
{
    public partial class MainTest
    {
        #region DO NOT run without asking Karen or reading the docs

        [TestMethod]
        [Ignore]
        [TestTraits(Trait.JsonGeneration)]
        public async Task CreateJsonFilesTask()
        {
            await SerializeModelsToJson();
        }

        [TestMethod]
        [Ignore]
        [TestTraits(Trait.JsonGeneration)]
        public async Task CreateAlternateContactsJsonFilesTask()
        {
            await SerializeContactsAlternateToJson();
        }
        [TestMethod]
        [Ignore]
        [TestTraits(Trait.JsonGeneration)]
        public async Task CreateOrdersJsonFilesTask()
        {
            await SerializeOrdersToJson();
        }

        #endregion

    }
}
