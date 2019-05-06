using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using EntityFrameworkMigrator.Extensions;
using EntityFrameworkMigrator.Mappers;
using EntityFrameworkMigrator.SqlHelpers;

namespace EntityFrameworkMigrator.Entities
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

        public EntityDatabaseMap BuildEntityDatabaseMap(DbContext dbContext)
        {
            var map = new EntityDatabaseMap();

            map.DatabaseName = dbContext.GetDatabaseName();

            var metadata = dbContext.GetMetadata();

            var entityTypes = metadata.GetEntityTypes();

            var entitiesInfo = entityTypes.Select(entityType => new EntityInformation
                {
                    ClrType = entityType,
                    TableInformation = metadata.GetTableInfo(entityType),
                    ForeignKeys = GetForeignKeys(dbContext, metadata, entityType),
                    PrimaryKeys = GetPrimaryKeys(dbContext, metadata, entityType),
                    Indices = GetIndices(dbContext, metadata, entityType)
                })
                .ToList();

            map.EntityInformation = entitiesInfo;

            return map;
        }

        private IEnumerable<PrimaryKey> GetPrimaryKeys(
            DbContext dbContext,
            MetadataWorkspace metadata,
            Type entityType)
        {
            var tableInfo = metadata.GetTableInfo(entityType);
            var databasePrimaryKeyInfo = _sqlHelper.GetPrimaryKeyInformation(dbContext, tableInfo.Name);
            var reflectionPrimaryKeyInfo = metadata.GetPrimaryKeys(entityType);
            var primaryKeys = _relationshipMapper.MapPrimaryKeys(databasePrimaryKeyInfo, reflectionPrimaryKeyInfo);

            foreach (var primaryKey in primaryKeys)
            {
                primaryKey.Table = tableInfo;
            }

            return primaryKeys;
        }

        private IEnumerable<Index> GetIndices(
            DbContext dbContext,
            MetadataWorkspace metadata,
            Type entityType)
        {
            var tableInfo = metadata.GetTableInfo(entityType);
            var databaseIndexInfo = _sqlHelper.GetIndexInformation(dbContext, tableInfo.Name);
            var indexes = _relationshipMapper.MapIndexes(databaseIndexInfo, tableInfo);

            return indexes;
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
