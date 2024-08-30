using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosLayer
{
    public class Customers //Aquí se define la clase Customers
                           //Esta clase es un modelo de datos que encapsula la
                           //información relacionada con un cliente.
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }//Cada una de estas líneas define una propiedad pública de la clase Customers
     //Las propiedades utilizan la sintaxis de propiedad automática (get; set;),
     //lo que significa que C# generará automáticamente un campo privado y los métodos get y set
     //para cada propiedad.
}
