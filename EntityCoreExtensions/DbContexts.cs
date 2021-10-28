﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EntityCoreExtensions.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityCoreExtensions
{
    public static class DbContexts
    {
        /// <summary>
        /// Get model names for a <see cref="DbContext"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<string> GetModelNames(this DbContext context) =>
            context.ModelTypeInformation().Select(item => item.Name).ToList();

        /// <summary>
        /// Get models details for a <see cref="DbContext"/>
        /// </summary>
        /// <param name="context"><see cref="DbContext"/></param>
        /// <returns>List&lt;<see cref="Type"/>> for each model</returns>
        public static List<Type> ModelTypeInformation(this DbContext context)
        {
            return context.Model.GetEntityTypes().Select(entityType => entityType.ClrType).ToList();
        }
        /// <summary>
        /// Get details for a model
        /// </summary>
        /// <param name="context">Active dbContext</param>
        /// <param name="modelName">Model name in context</param>
        /// <returns>List&lt;SqlColumn&gt;</returns>
        public static List<SqlColumn> GetEntityProperties([NotNull] this DbContext context, string modelName)
        {

            var entityType = GetEntityType(context, modelName);
            var sqlColumnsList = new List<SqlColumn>();

            IEnumerable<IProperty> properties = context.Model.FindEntityType(entityType ?? throw new InvalidOperationException()).GetProperties();

            foreach (IProperty itemProperty in properties)
            {
                var sqlColumn = new SqlColumn() { Name = itemProperty.Name };
                var comment = context.Model.FindEntityType(entityType).FindProperty(itemProperty.Name).GetComment();

                sqlColumn.Description = string.IsNullOrWhiteSpace(comment) ? itemProperty.Name : comment;

                sqlColumn.IsPrimaryKey = itemProperty.IsKey();
                sqlColumn.IsForeignKey = itemProperty.IsForeignKey();
                sqlColumn.IsNullable = itemProperty.IsColumnNullable();

                sqlColumnsList.Add(sqlColumn);

            }

            return sqlColumnsList;

        }
        /// <summary>
        /// Get type from model name
        /// </summary>
        /// <param name="context">Live DbContext</param>
        /// <param name="modelName">Valid model name</param>
        /// <returns></returns>
        private static Type GetEntityType([NotNull] DbContext context, string modelName)
        {
            var entityType = context.Model.GetEntityTypes().Select(eType => eType.ClrType).FirstOrDefault(type => type.Name == modelName);

            return entityType;

        }

        /// <summary>
        /// Get model comments by model type
        /// </summary>
        /// <param name="context">Live DbContext</param>
        /// <param name="modelType">Model type</param>
        /// <returns>IEnumerable&lt;ModelComment&gt;</returns>
        /// <remarks>
        /// context.Comments(typeof(Customers));
        /// </remarks>
        public static IEnumerable<ModelComment> Comments([NotNull] this DbContext context, Type modelType)
        {

            IEntityType entityType = context.Model.FindRuntimeEntityType(modelType);

            return entityType.GetProperties().Select(property => new ModelComment
            {
                Name = property.Name,
                Comment = property.GetComment()
            });

        }
        /// <summary>
        /// Returns a list of column names for model
        /// </summary>
        /// <param name="context">Live DbContext</param>
        /// <param name="modelName">Existing model name</param>
        /// <returns></returns>
        public static List<string> ColumnNames([NotNull] this DbContext context, string modelName)
        {

            var entityType = GetEntityType(context, modelName);
            var sqlColumnsList = new List<string>();

            IEnumerable<IProperty> properties = context.Model.FindEntityType(entityType ?? throw new InvalidOperationException()).GetProperties();

            foreach (IProperty itemProperty in properties)
            {
                sqlColumnsList.Add(itemProperty.Name);
            }

            return sqlColumnsList;


        }
        /// <summary>
        /// Get comments for a specific model
        /// </summary>
        /// <param name="context">Live DbContext</param>
        /// <param name="modelName">Model name to get comments for</param>
        /// <returns>IEnumerable&lt;ModelComment&gt;</returns>
        /// <remarks>
        /// context.Comments("Customers");
        /// </remarks>
        public static IEnumerable<ModelComment> Comments([NotNull] this DbContext context, string modelName)
        {
            var entityType = GetEntityType(context, modelName);

            IEnumerable<IProperty> properties = context.Model.FindEntityType(entityType ?? throw new InvalidOperationException()).GetProperties();

            return properties.Select(itemProperty => new ModelComment
            {
                Name = itemProperty.Name, 
                Comment = context.Model.FindEntityType(entityType).FindProperty(itemProperty.Name).GetComment() ?? itemProperty.Name
            }).ToList();

        }
    }

}