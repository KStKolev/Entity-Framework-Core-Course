using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.ProductShop.DTO.Export
{
    public class ExportUserDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public virtual ExportUsersAndProductsDTO[] Users { get; set; }
    }
}
