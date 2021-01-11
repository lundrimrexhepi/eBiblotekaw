using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBibloteka.Models
{
    public class PerdoruesiListViewModel
    {
        public int PerdoruesiID { get; set; }
        public string NumriPersonal { get; set; }
        public string Emri { get; set; }
        public string Mbiemri { get; set; }
        public byte[] Fjalkalimi { get; set; }
        public int KomunaID { get; set; }
        public string Adresa { get; set; }
        public string Email { get; set; }
        public string Telefoni { get; set; }
        public Nullable<System.DateTime> DateLindja { get; set; }
        public int GrupiID { get; set; }
        public System.DateTime DataInsertimit { get; set; }
        public string Perdoruesi { get; set; }
    }
}