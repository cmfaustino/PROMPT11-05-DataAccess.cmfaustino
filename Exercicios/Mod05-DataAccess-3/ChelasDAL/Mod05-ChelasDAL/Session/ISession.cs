namespace Mod05_ChelasDAL.Session
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using Mod05_ChelasDAL.Metadata;

    public interface ISession : IDisposable
    {
        void Commit();
        void Rollback();

        //IQuery CreateQuery(string sql);
        //IQuery CreateQuery<TEntity>(string whereClause);

        TEntity Get<TEntity>(object id);
        IEnumerable<TEntity> FindAll<TEntity>();

        TEntity Insert<TEntity>(TEntity entity);
        TEntity Update<TEntity>(TEntity entity);
        void Delete<TEntity>(TEntity entity);

        TableInfo GetTableInfoFor<TEntity>();

        void ClearCache();
        void RemoveFromCache(object entity);
        void RemoveAllInstancesFromCache<TEntity>();

        SqlConnection GetConnection();
        SqlTransaction GetTransaction();
    }
}