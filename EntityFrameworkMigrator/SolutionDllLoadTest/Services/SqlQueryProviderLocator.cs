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
            var generator = generators[dbmsType.GetType()];

            return generator;
        }

        public static ISqlHelper ResolveSqlHelper(DbProviderFactory dbmsType)
        {
            var helper = helpers[dbmsType.GetType()];

            return helper;
        }

        public static void AssertSupported(DbProviderFactory dbmsType)
        {
            var type = dbmsType.GetType();
            if (!helpers.TryGetValue(type, out var helper) && !generators.TryGetValue(type, out var generator))
            {
                throw new NotImplementedException();
            }
        }
    }
}
