using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBibloteka.Models
{
    public class Huazimi
    {
        public int HuazimiID { get; set; }
        public int PerdoruesiID { get; set; }
        public int PerdoruesiIDLexuesi { get; set; }
        public int LibriID { get; set; }
        public int? Sasia { get; set; }
        public DateTime DataHuazimit { get; set; }
        public DateTime DataKthimit { get; set; }
        public string Shenime { get; set; }
        public string Lexuesi { get; set; }
        public string Libri { get; set; }
    }
}