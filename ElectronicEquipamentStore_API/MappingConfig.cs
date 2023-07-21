using AutoMapper;
using ElectronicEquipamentStore_API.Models;
using ElectronicEquipamentStore_API.Models.Dto;

namespace ElectronicEquipamentStore_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<ElectronicEquipament, ElectronicEquipamentDto>().ReverseMap();

            CreateMap<ElectronicEquipament, ElectronicEquipamentCreateDto>().ReverseMap();

            CreateMap<ElectronicEquipament, ElectronicEquipamentUpdateDto>().ReverseMap();
        }
    }
}
