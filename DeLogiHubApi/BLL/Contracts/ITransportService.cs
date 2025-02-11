using BLL.Infrastructure.Models;

namespace BLL.Contracts;
public interface ITransportService
{
	List<TransportModel> GetAllByUserId(Guid userId);

	TransportModel GetById(Guid transportId);

	void Add(TransportModel transportModel);

	void Update(TransportModel transportModel);

	void Delete(Guid transportId);
}
