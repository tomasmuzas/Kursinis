using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using SolutionDllLoadTest.RelationshipDTOs;

namespace SolutionDllLoadTest.Extensions
{
    public static class MetadataExtensions
    {
        public static IEnumerable<Type> GetEntityTypes(this MetadataWorkspace metadata)
        {
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            var entityTypes = objectItemCollection
                .GetItems<EntityType>()
                .Select(i => objectItemCollection.GetClrType(i));

            return entityTypes;
        }

        public static Type GetTypeFromName(this MetadataWorkspace metadata, string entityName)
        {
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            return objectItemCollection
                .GetItems<EntityType>()
                .Select(i => objectItemCollection.GetClrType(i))
                .SingleOrDefault(t => t.Name == entityName);
        }

        public static string GetTableName(this MetadataWorkspace metadata, Type entityType)
        {
                // Get the part of the model that contains info about the actual CLR types
                var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

                // Get the entity type from the model that maps to the CLR type
                var entityClrType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == entityType);

                // Get the entity set that uses this entity type
                var entitySet = metadata
                    .GetItems<EntityContainer>(DataSpace.CSpace)
                    .Single()
                    .EntitySets
                    .Single(s => s.ElementType.Name == entityClrType.Name);

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

        public static string[] GetKeyNames(this MetadataWorkspace metadata, Type entityType)
        {
            // Get the mapping between CLR types and metadata OSpace
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get metadata for given CLR type
            var entityMetadata = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == entityType);

            return entityMetadata.KeyProperties.Select(p => p.Name).ToArray();
        }

        public static IEnumerable<ForeignKey> GetForeignKeys(this MetadataWorkspace metadata, Type entityType)
        {
            var fk = metadata
                .GetItems<AssociationType>(DataSpace.SSpace)
                .SelectMany(a => a.ReferentialConstraints)
                .Where(rc => rc.ToRole.Name == entityType.Name)
                .Select(rc => new ForeignKey
                {
                    FromEntity = metadata.GetTypeFromName(rc.ToRole.Name),
                    FromColumn = rc.ToProperties[0].Name,
                    ToEntity = metadata.GetTypeFromName(rc.FromRole.Name),
                    ToColumn = rc.FromProperties[0].Name
                })
                .ToList();
            return fk;
        }
    }
}
