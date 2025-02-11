using DAL.Contracts;
using DAL.DbContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories;
public class Repository<TEntity> : IRepository<TEntity>
	where TEntity : BaseEntity
{
	private readonly DeLogiHubDbContext _dbContext;
	private readonly DbSet<TEntity> _dbSet;

	public Repository(DeLogiHubDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = _dbContext.Set<TEntity>();
	}

	public void Add(TEntity entity)
	{
		_dbSet.Add(entity);
		_dbContext.SaveChanges();
	}

	public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
	{
		return _dbSet.FirstOrDefault(predicate);
	}

	public IQueryable<TEntity> GetAll()
	{
		return _dbSet.AsQueryable();
	}

	public TEntity? GetById(Guid id)
	{
		return _dbSet.Find(id);
	}

	public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
	{
		return _dbSet.Where(predicate);
	}

	public void Remove(TEntity entity)
	{
		_dbSet.Remove(entity);
		_dbContext.SaveChanges();
	}

	public void Update(TEntity entity)
	{
		_dbSet.Update(entity);
		_dbContext.SaveChanges();
	}
}
