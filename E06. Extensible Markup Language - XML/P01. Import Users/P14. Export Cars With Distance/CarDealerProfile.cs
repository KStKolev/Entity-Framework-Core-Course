using AutoMapper;
using CarDealer.CarDealer.DTO.Export;
using CarDealer.CarDealer.DTO.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Import
            CreateMap<ImportSuppliersDTO, Supplier>();
            CreateMap<ImportPartsDTO, Part>();
            CreateMap<ImportPartToCar, ImportCarsDTO>();
            CreateMap<ImportCarsDTO, Car>();
            CreateMap<ImportCustomersDTO, Customer>();
            CreateMap<ImportSalesDTO, Sale>();

            //Export
            CreateMap<Car, ExportCarsWithDistance>();
        }
    }
}
