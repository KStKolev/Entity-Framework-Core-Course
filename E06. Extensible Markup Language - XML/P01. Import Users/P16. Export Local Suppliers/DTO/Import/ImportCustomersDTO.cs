using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.CarDealer.DTO.Import
{
    [XmlType("Customer")]
    public class ImportCustomersDTO
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("birthDate")]
        public DateTime BirthDay { get; set; }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
