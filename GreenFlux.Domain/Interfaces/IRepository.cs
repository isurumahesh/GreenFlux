namespace GreenFlux.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task Delete(T entity);

        public Task Update(T entity);
    }
}