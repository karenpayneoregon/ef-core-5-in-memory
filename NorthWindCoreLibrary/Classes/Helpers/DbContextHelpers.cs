using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NorthWindCoreLibrary.Data;

namespace NorthWindCoreLibrary.Classes.Helpers
{
    /// <summary>
    /// Un tested code
    /// </summary>
    public static class DbContextHelpers
    {
        /// <summary>
        /// Generic reload for consistency 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="modelEntityEntry"></param>
        public static void Reload(this DbContext context, EntityEntry modelEntityEntry)
        {
            context.Entry(modelEntityEntry).Reload();
        }
        
        /// <summary>
        /// Kind of drastic and not recommended 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task Refresh(this DbContext context)
        {
            await context.Database.CloseConnectionAsync();
            await context.Database.OpenConnectionAsync();
        }
        public static async Task<bool> TestConnectionTask(this DbContext context)
            => await Task.Run(async () => await context.Database.CanConnectAsync());

        public static async Task<bool> TestConnectionTask(this DbContext context, CancellationToken token)
            => await Task.Run(async () => await context.Database.CanConnectAsync(token), token);
    }
}
