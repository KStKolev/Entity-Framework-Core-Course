using AutoMapper;
using CarDealer.CarDealer.DTO.Export;
using CarDealer.CarDealer.DTO.Export.SubExportClasses;
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
            CreateMap<Car, ExportCarsFromMakeBMWDTO>();
            CreateMap<Supplier, ExportLocalSuppliersDTO>();
            CreateMap<Car, ExportCarsWithTheirListOfPartsDTO>();
            CreateMap<ExportListOfPartsDTO, ExportCarsWithTheirListOfPartsDTO>();
            CreateMap<ListOfParts, ExportListOfPartsDTO>();
            CreateMap<ExportTotalSalesByCustomerDTO, Customer>();
        }
    }
}
