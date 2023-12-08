using CarDealer.CarDealer.DTO.Export.SubExportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.CarDealer.DTO.Export
{
    [XmlType("sale")]
    public class ExportSalesWithAppliedDiscountDTO
    {
        [XmlElement("car")]
        public CarInSale Car { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceDiscount { get; set;}
    }
}
