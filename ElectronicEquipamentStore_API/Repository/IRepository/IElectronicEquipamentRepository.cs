using ElectronicEquipamentStore_API.Models;

namespace ElectronicEquipamentStore_API.Repository.IRepository
{
    public interface IElectronicEquipamentRepository : IRepositroy<ElectronicEquipament>
    {
        Task<ElectronicEquipament> Update(ElectronicEquipament entity);
    }
}
