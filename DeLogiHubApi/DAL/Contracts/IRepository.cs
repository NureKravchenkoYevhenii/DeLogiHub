using Domain.Models;
using System.Linq.Expressions;

namespace DAL.Contracts;
public interface IRepository<TEntity>
	where TEntity : BaseEntity
{
	void Add(TEntity entity);

	TEntity? GetById(Guid id);

	TEntity? Get(Expression<Func<TEntity, bool>> predicate);

	IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

	IQueryable<TEntity> GetAll();

	void Update(TEntity entity);

	void Remove(TEntity entity);
}
