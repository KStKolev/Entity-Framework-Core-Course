using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.CarDealer.DTO.Import
{
    [XmlType("partId")]
    public class ImportPartToCar
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
