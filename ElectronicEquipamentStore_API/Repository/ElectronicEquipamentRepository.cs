using ElectronicEquipamentStore_API.Data;
using ElectronicEquipamentStore_API.Models;
using ElectronicEquipamentStore_API.Repository.IRepository;

namespace ElectronicEquipamentStore_API.Repository
{
    public class ElectronicEquipamentRepository : Repository<ElectronicEquipament>, IElectronicEquipamentRepository
    {
        private readonly ApplicationDbContext _db;

        public ElectronicEquipamentRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public async Task<ElectronicEquipament> Update(ElectronicEquipament entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
