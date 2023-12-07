using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.ProductShop.DTO.Export
{
    [XmlType("User")]
    public class ExportUserWithProductDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public virtual ExportBoughtProductDTO[] ProductsSold { get; set; }
    }
}
