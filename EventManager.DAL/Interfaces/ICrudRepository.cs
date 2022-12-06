namespace EventManager.DAL.Interfaces
{
    public interface ICrudRepository<TId, TEntity>
        where TEntity : class
    {
        TEntity? GetById(TId id);
        IEnumerable<TEntity> GetAll();

        TId Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TId id);
    }
}
