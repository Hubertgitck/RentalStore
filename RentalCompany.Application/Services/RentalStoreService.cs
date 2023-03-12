using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Application.Middleware.CustomExceptions;
using RentalCompany.Infrastructure.Models;
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
        var allStoresCollection = _unitOfWork.RentalStore.GetAll(includeProperties: "ContactData");
        var allStoresCollectionDto = _mapper.Map<IEnumerable<RentalStoreDto>>(allStoresCollection);

        return Task.FromResult(allStoresCollectionDto);
    }

    public Task AddRentalStore(RentalStoreDto rentalStoreDto)
    {
        var rentalStoreToAddToDb = _mapper.Map<RentalStore>(rentalStoreDto);
        _unitOfWork.RentalStore.Add(rentalStoreToAddToDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task<RentalStoreDto> GetRentalStoreById(int? id)
    {
        if (id.GetValueOrDefault() == 0)
        {
            throw new ArgumentException("Invalid id");
        }

        var rentalStoreFromDb = _unitOfWork.RentalStore
            .GetFirstOrDefault(c => c.Id == id, includeProperties: "ContactData");

        if (rentalStoreFromDb == null)
        {
            throw new NotFoundException($"Rental Store with ID: {id} was not found in database");
        }

        var rentalStoreDto = _mapper.Map<RentalStoreDto>(rentalStoreFromDb);

        return Task.FromResult(rentalStoreDto);
    }

    public Task EditRentalStore(RentalStoreDto rentalStoreDto)
    {
        var rentalStoreToAddToDb = _mapper.Map<RentalStore>(rentalStoreDto);

        _unitOfWork.RentalStore.Update(rentalStoreToAddToDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task DeleteRentalStoreById(int? id)
    {
        var rentalStoreToDeleteFromDb = _unitOfWork.RentalStore.GetFirstOrDefault(u => u.Id == id);

        if (rentalStoreToDeleteFromDb == null)
        {
            throw new NotFoundException($"Rental store with ID: {id} was not found in database");
        }
        _unitOfWork.RentalStore.Remove(rentalStoreToDeleteFromDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }
}
