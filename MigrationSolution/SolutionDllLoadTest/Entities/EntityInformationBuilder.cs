using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using SolutionDllLoadTest.Extensions;
using SolutionDllLoadTest.Mappers;
using SolutionDllLoadTest.SqlHelpers;

namespace SolutionDllLoadTest.Entities
{
    public class EntityInformationBuilder
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly RelationshipMapper _relationshipMapper;

        public EntityInformationBuilder(
            ISqlHelper sqlHelper,
            RelationshipMapper relationshipMapper)
        {
            _sqlHelper = sqlHelper;
            _relationshipMapper = relationshipMapper;
        }

        public IEnumerable<EntityInformation> BuildEntityInformationFromAssembly(Assembly assembly)
        {
            var dbContextType = assembly.DefinedTypes
                .Single(t => t.IsSubclassOf(typeof(DbContext)));
            var dbContextInstance = (DbContext)Activator.CreateInstance(dbContextType);

            var metadata = dbContextInstance.GetMetadata();

            var entitiesInfo = new List<EntityInformation>();

            var entityTypes = metadata.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var entity = new EntityInformation
                {
                    ClrType = entityType,
                    TableInformation = metadata.GetTableInfo(entityType),
                    ForeignKeys = GetForeignKeys(dbContextInstance, metadata, entityType)
                };

                entitiesInfo.Add(entity);
            }

            return entitiesInfo;
        }

        private IEnumerable<ForeignKey> GetForeignKeys(
            DbContext dbContext, 
            MetadataWorkspace metadata, 
            Type entityType)
        {
            var tableInfo = metadata.GetTableInfo(entityType);
            var databaseForeignKeyInfo = _sqlHelper.GetForeignKeyInformation(dbContext, tableInfo.Schema, tableInfo.Name);
            var reflectionForeignKeyInfo = metadata.GetForeignKeys(entityType);
            var foreignKeys = _relationshipMapper.MapForeignKeys(databaseForeignKeyInfo, reflectionForeignKeyInfo);

            return foreignKeys;
        }
    }
}
