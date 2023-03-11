using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Application.Services;

public class RentalStoreService : IRentalStoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RentalStoreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<IEnumerable<RentalStoreDto>> GetAllRentalStores()
    {
        var allStoresCollection = _unitOfWork.RentalStore.GetAll();
        var allStoresCollectionDto = _mapper.Map<IEnumerable<RentalStoreDto>>(allStoresCollection);

        return Task.FromResult(allStoresCollectionDto);
    }

}
