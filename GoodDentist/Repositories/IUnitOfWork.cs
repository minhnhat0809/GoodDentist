namespace Repositories;

public interface IUnitOfWork
{
    IAccountRepo Account { get; }
    IClinicRepository   Clinic { get; }
    IRoleRepo Role { get; }
    IDentistSlotRepository DentistSlot { get; }
    void Save();

}