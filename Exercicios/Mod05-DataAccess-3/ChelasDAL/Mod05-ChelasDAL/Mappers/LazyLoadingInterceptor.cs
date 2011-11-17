namespace Mod05_ChelasDAL.Mappers
{
    using Castle.DynamicProxy;

    using Mod05_ChelasDAL.Metadata;

    public class LazyLoadingInterceptor : IInterceptor
    {
        private TableInfo _tableInfo;

        private readonly Session _session;

        private bool _needsToBeInitialized = true;

        public LazyLoadingInterceptor(TableInfo tableInfo, Session session)
        {
            this._tableInfo = tableInfo;
            this._session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.Equals("get_" + this._tableInfo.PrimaryKey.PropertyInfo.Name)
                || invocation.Method.Name.Equals("set_" + this._tableInfo.PrimaryKey.PropertyInfo.Name))
            {
                invocation.Proceed();
                return;
            }

            if (this._needsToBeInitialized)
            {
                this._needsToBeInitialized = false;
                this._session.InitializeProxy(invocation.Proxy, invocation.TargetType);
            }

            invocation.Proceed();
        }
    }
}