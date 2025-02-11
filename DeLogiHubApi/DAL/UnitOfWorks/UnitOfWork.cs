using DAL.Contracts;
using DAL.DbContexts;
using DAL.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.UnitOfWorks;
public class UnitOfWork : IUnitOfWork
{
	private readonly DeLogiHubDbContext _dbContext;
	private Dictionary<Type, object> _repositories;
	private bool _disposed = false;

	public UnitOfWork(DeLogiHubDbContext dbContext)
	{
		_dbContext = dbContext;
		_repositories = [];
	}

	public IDbContextTransaction BeginTransaction()
	{
		return _dbContext.Database.BeginTransaction();
	}

	public void Commit()
	{
		_dbContext.SaveChanges();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_dbContext.Dispose();
			}

			_disposed = true;
		}
	}

	public Lazy<IRepository<TEntity>> GetLazyRepository<TEntity>() where TEntity : BaseEntity
	{
		return new Lazy<IRepository<TEntity>>(GetRepository<TEntity>);
	}

	public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
	{
		if (_repositories.ContainsKey(typeof(TEntity)))
		{
			return (IRepository<TEntity>)_repositories[typeof(TEntity)];
		}

		var repository = new Repository<TEntity>(_dbContext);
		_repositories.Add(typeof(TEntity), repository);

		return repository;
	}

	public int ExecuteSqlRaw(string sql, params object[] parameters)
	{
		return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
	}
}
