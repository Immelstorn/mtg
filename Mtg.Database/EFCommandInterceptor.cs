using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Mtg.Database
{
    public class EFCommandInterceptor : IDbCommandInterceptor
    {
        public void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        public void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

    }
}