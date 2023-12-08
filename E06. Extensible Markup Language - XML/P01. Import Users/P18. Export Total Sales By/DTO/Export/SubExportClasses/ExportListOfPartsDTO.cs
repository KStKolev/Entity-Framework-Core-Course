using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.CarDealer.DTO.Export.SubExportClasses
{
    public class ExportListOfPartsDTO
    {
        [XmlElement("part")]
        public virtual ListOfParts[] Parts { get; set; }
    }
}
