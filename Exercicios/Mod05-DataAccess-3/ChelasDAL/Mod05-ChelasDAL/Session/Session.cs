namespace Mod05_ChelasDAL.Session
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;

    using Mod05_ChelasDAL.Mappers;
    using Mod05_ChelasDAL.Metadata;

    public class Session : ISession
    {
        private readonly string connectionString;
        private SqlConnection connection;
        private SqlTransaction transaction;
        private readonly MetaDataStore _metaDataStore;
        private readonly EntityHydrater hydrater;
        private readonly IdentityMap _identityMap;

        Dictionary<Type, IMapper> _entityMappers = new Dictionary<Type, IMapper>(); 

        public Session(string connectionString, MetaDataStore metaDataStore)
        {
            this.connectionString = connectionString;
            this._metaDataStore = metaDataStore;
            this._identityMap = new IdentityMap();
            hydrater = new EntityHydrater(metaDataStore, _identityMap);

            FillEntityMappers();
        }

        private void FillEntityMappers()
        {
            // TODO
            //Type t = this.GetType();
            //IEnumerable<PropertyInfo> mappers = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => typeof(). pi.PropertyType.IsMapperInterfaceImpl
        }

        private void InitializeConnection()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        public SqlConnection GetConnection()
        {
            if (connection == null)
            {
                InitializeConnection();
            }

            return connection;
        }

        public SqlTransaction GetTransaction()
        {
            if (transaction == null)
            {
                InitializeConnection();
            }

            return transaction;
        }

        //public IQuery CreateQuery(string sql)
        //{
        //    var command = GetConnection().CreateCommand();
        //    command.Transaction = GetTransaction();
        //    command.CommandText = sql;
        //    return new Query(command, _metaDataStore, hydrater);
        //}
        //public IQuery CreateQuery<TEntity>(string whereClause)
        //{
        //    return CreateQuery(_metaDataStore.GetTableInfoFor<TEntity>().GetSelectStatementForAllFields() + " " + whereClause);
        //}

        public TableInfo GetTableInfoFor<TEntity>()
        {
            return this._metaDataStore.GetTableInfoFor<TEntity>();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            if (transaction != null) transaction.Dispose();
            if (connection != null) connection.Dispose();
        }

        private IMapper GetMapperForType(Type t)
        {
            IMapper mapper = _entityMappers[t];
            return mapper;
        }

        public TEntity Get<TEntity>(object id)
        {
            return CreateAction<GetByIdAction>().Get<TEntity>(id);
        }

        public IEnumerable<TEntity> FindAll<TEntity>()
        {
            return CreateAction<FindAllAction>().FindAll<TEntity>();
        }

        public TEntity Insert<TEntity>(TEntity entity)
        {
            return CreateAction<InsertAction>().Insert(entity);
        }

        public TEntity Update<TEntity>(TEntity entity)
        {
            return CreateAction<UpdateAction>().Update(entity);
        }

        public void Delete<TEntity>(TEntity entity)
        {
            CreateAction<DeleteAction>().Delete(entity);
        }

        public void InitializeProxy(object proxy, Type targetType)
        {
            CreateAction<InitializeProxyAction>().InitializeProxy(proxy, targetType);
        }
        public void ClearCache()
        {
            this._identityMap.ClearAll();
        }

        public void RemoveFromCache(object entity)
        {
            this._identityMap.Remove(entity);
        }

        public void RemoveAllInstancesFromCache<TEntity>()
        {
            this._identityMap.RemoveAllInstancesOf(typeof(TEntity));
        }

    }
}