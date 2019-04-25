using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace SolutionDllLoadTest
{
    public class Program
    {
        public static string[] GetForeignKeys(DbContext context, Type type)
        {
            StructuralType edmType = GetCSpaceType(context, type);

            var members = edmType
                .MetadataProperties
                .FirstOrDefault(mp => mp.Name == "Members");
            if (members != null && members.Value != null)
            {
                List<NavigationProperty> navProps = ((ICollection<EdmMember>)members.Value)
                    .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.NavigationProperty)
                    .Cast<NavigationProperty>()
                    .Where(p =>
                        ((AssociationType)p.RelationshipType).IsForeignKey
                    )
                    .ToList();
                List<EdmProperty> foreignKeys = navProps
                    .SelectMany(p => p.GetDependentProperties())
                    .ToList();
                return foreignKeys.Select(p => p.Name).ToArray();
            }
            return null;
        }

        public static string GetTableName(Type type, DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return (string)table.MetadataProperties["Schema"].Value + "." + (string)table.MetadataProperties["Table"].Value /*?? table.Schema + "." + table.Name*/;
        }

        public static string[] GetKeyNames(DbContext context, Type entityType)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the mapping between CLR types and metadata OSpace
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get metadata for given CLR type
            var entityMetadata = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == entityType);

            return entityMetadata.KeyProperties.Select(p => p.Name).ToArray();
        }

        private static StructuralType GetCSpaceType(DbContext context, Type type)
        {
            MetadataWorkspace workspace = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            EdmType ospaceType = workspace.GetType(type.Name, type.Namespace, DataSpace.OSpace);
            return workspace.GetEdmSpaceType((StructuralType)ospaceType);
        }

        static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom("C:\\Users\\BD1892\\source\\repos\\Kursinis\\MigrationDemo\\bin\\MigrationDemo.dll");
            var dbContextType = assembly.DefinedTypes
                .Single(t => t.IsSubclassOf(typeof(DbContext)));

            var dbContextInstance = (DbContext)Activator.CreateInstance(dbContextType);

            var objectContext = ((IObjectContextAdapter) dbContextInstance).ObjectContext;

            var metadata = objectContext.MetadataWorkspace;

            var items = metadata.GetItems(DataSpace.OCSpace);

            var fk = metadata.GetItems<AssociationType>(DataSpace.SSpace);

            var entityTypes = dbContextType.DeclaredProperties.
                Where(p => p.GetGetMethod().IsVirtual
                && p.PropertyType.IsGenericType
                && p.PropertyType.GetGenericTypeDefinition().IsEquivalentTo(typeof(DbSet<>)))
                .Select(p => p.PropertyType.GetGenericArguments()[0])
                .ToList();

            // FOREIGN KEY USES PATTERN UNDERNEATH, UNLESS NAME IS PROVIDED

            foreach (var entityType in entityTypes)
            {
                Console.WriteLine(string.Join(",", GetTableName(entityType, dbContextInstance)));
                Console.WriteLine(string.Join(",", GetKeyNames(dbContextInstance, entityType)));
            }
        }
    }
}
