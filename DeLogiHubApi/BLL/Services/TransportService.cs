using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Exceptions;

namespace BLL.Services;
public class TransportService : ITransportService
{
	private readonly Lazy<IUnitOfWork> _unitOfWork;
	private readonly Lazy<IMapper> _mapper;
	private readonly Lazy<IRepository<Transport>> _transports;

	public TransportService(
		Lazy<IUnitOfWork> unitOfWork,
		Lazy<IMapper> mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;

		_transports = _unitOfWork.Value.GetLazyRepository<Transport>();
	}

	public void Add(TransportModel transportModel)
	{
		var transport = _mapper.Value.Map<Transport>(transportModel);

		_transports.Value.Add(transport);
	}

	public void Delete(Guid transportId)
	{
		var transport = _transports.Value.GetById(transportId);
		if (transport == null)
			return;

		_transports.Value.Remove(transport);
	}

	public List<TransportModel> GetAllByUserId(Guid userId)
	{
		var userTransports = _transports.Value
			.GetAll()
			.Where(t => t.CarrierId == userId)
			.ToList();
		var userTransportsModel = _mapper.Value.Map<List<TransportModel>>(userTransports);

		return userTransportsModel;
	}

	public TransportModel GetById(Guid transportId)
	{
		var transport = _transports.Value.GetById(transportId)
			?? throw new EntityNotFoundException("Транспортний засіб не знайдено");
		var transportModel = _mapper.Value.Map<TransportModel>(transport);

		return transportModel;
	}

	public void Update(TransportModel transportModel)
	{
		var transport = _transports.Value.GetById(transportModel.Id)
			?? throw new EntityNotFoundException("Транспортний засіб не знайдено");
		_mapper.Value.Map(transportModel, transport);
		_transports.Value.Update(transport);
	}
}
