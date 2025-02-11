using Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Contracts;
public interface IUnitOfWork : IDisposable
{
	void Commit();

	IRepository<TEntity> GetRepository<TEntity>()
		where TEntity : BaseEntity;

	Lazy<IRepository<TEntity>> GetLazyRepository<TEntity>()
		where TEntity : BaseEntity;

	IDbContextTransaction BeginTransaction();

	int ExecuteSqlRaw(string sql, params object[] parameters);
}
