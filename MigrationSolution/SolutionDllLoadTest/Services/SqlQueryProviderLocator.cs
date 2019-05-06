using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using EntityFrameworkMigrator.QueryGenerators;
using EntityFrameworkMigrator.SqlHelpers;

namespace EntityFrameworkMigrator.Services
{
    public static class SqlQueryProviderLocator
    {
        private static readonly Dictionary<Type, IQueryGenerator> generators = new Dictionary<Type, IQueryGenerator>
        {
            { typeof(SqlClientFactory) , new SqlServerQueryGenerator() }
        };

        private static readonly Dictionary<Type, ISqlHelper> helpers = new Dictionary<Type, ISqlHelper>
        {
            { typeof(SqlClientFactory) , new SqlServerHelper() }
        };

        public static IQueryGenerator ResolveQueryProvider(DbProviderFactory dbmsType)
        {
            var exists = generators.TryGetValue(dbmsType.GetType(), out var generator);

            if (!exists)
            {
                throw new NotImplementedException();
            }

            return generator;
        }

        public static ISqlHelper ResolveSqlHelper(DbProviderFactory dbmsType)
        {
            var exists = helpers.TryGetValue(dbmsType.GetType(), out var helper);

            if (!exists)
            {
                throw new NotImplementedException();
            }

            return helper;
        }
    }
}
