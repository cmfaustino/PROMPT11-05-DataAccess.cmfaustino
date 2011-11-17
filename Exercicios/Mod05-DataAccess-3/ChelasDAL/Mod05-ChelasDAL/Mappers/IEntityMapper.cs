namespace Mod05_ChelasDAL.Mappers
{
    using System.Collections.Generic;
    using System.Linq;



    public interface IEntityMapper<TEntity, TKey> : IMapper where TEntity: new()
    {
        IQueryable<TEntity> FindAll();
        TEntity GetById(TKey id);

        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
    }
}