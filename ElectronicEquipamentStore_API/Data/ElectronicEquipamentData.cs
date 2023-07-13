using ElectronicEquipamentStore_API.Models.Dto;

namespace ElectronicEquipamentStore_API.Data
{
    public static class ElectronicEquipamentData
    {
        public static List<ElectronicEquipamentDto> EquipamentList = new List<ElectronicEquipamentDto>
        {
                new ElectronicEquipamentDto {Id = 1, Name = "Contador", Description = "Contador de latas industrial"},
                new ElectronicEquipamentDto {Id = 2, Name = "Reciclador", Description = "Recicla aluminio y papel"}
        };
    }
}
