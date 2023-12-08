﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.ProductShop.DTO.Export
{
    [XmlType("SoldProducts")]
    public class SoldProductsDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public virtual ExportSoldProductsDTO[] SoldProducts { get; set; }
    }
}
