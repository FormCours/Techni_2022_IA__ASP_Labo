using EventManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.DAL.Repositories
{
    public abstract class RepositoryBase<TId, TEntity> : ICrudRepository<TId, TEntity>
        where TEntity: class
    {
        protected readonly IDbConnection connection;

        public RepositoryBase(IDbConnection dbConnection)
        {
            connection = dbConnection;
        }

        public abstract IEnumerable<TEntity> GetAll();
        public abstract TEntity? GetById(TId id);
        public abstract TId Insert(TEntity entity);
        public abstract bool Update(TEntity entity);
        public abstract bool Delete(TId id);
    }
}
