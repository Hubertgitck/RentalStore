namespace RentalCompany.Infrastructure.Repositories.Interfaces;
public interface ICarRepository : IRepository<Car>
{
    void Update(Car car);
}