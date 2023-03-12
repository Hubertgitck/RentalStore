namespace RentalCompany.Infrastructure.Repositories.Interfaces;
public interface IRentalStoreRepository : IRepository<RentalStore>
{
    public void Update(RentalStore store);
}
