using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.CarDealer.DTO.Export.SubExportClasses
{
    [XmlType("part")]
    public class ListOfParts
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = null!;

        [XmlAttribute("price")]
        public decimal Price { get; set; }

    }
}
