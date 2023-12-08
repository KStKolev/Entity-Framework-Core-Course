using AutoMapper;
using CarDealer.CarDealer.DTO.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportSuppliersDTO, Supplier>();
            CreateMap<ImportPartsDTO, Part>();
            CreateMap<ImportPartToCar, ImportCarsDTO>();
            CreateMap<ImportCarsDTO, Car>();
        }
    }
}
