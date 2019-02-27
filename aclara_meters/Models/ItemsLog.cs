
    /* 
  Licensed under the Apache License, Version 2.0

  http://www.apache.org/licenses/LICENSE-2.0
  */
    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;
namespace aclara_meters.Models
{

    public class ItemsLog
    {
        public string Icon { get; set; }
        public string Accion { get; set; }

        public List<DatosAccion> ListaDatos { get; set; }
        public List<ItemsLog> SubItemsLog { get; set; }

        public Boolean HayLista { get; set; }
       
    }
    public class DatosAccion
    {
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }
 }
