using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBibloteka.Models
{
    public class Librat
    {
        public string ISBN { get; set; }
        public string TittulliLibrit { get; set; }
        public int Sasia { get; set; }
        public int AutoriID { get; set; }
        public int GjuhaLibritID { get; set; }
        public string Botimi { get; set; }
        public int VitiBotimit { get; set; }
        public int NumriFaqeve { get; set; }
        public int ShtepiaBotuseID { get; set; }
        public int DhomaID { get; set; }
        public int RaftiID { get; set; }
        public string DonacionPershkrimi { get; set; }
        public int PerdoruesiIDLexuesit { get; set; }
        public int Rasti { get; set; }
        public DateTime DataInsertimi { get; set; }
        public int PerdoruesiID { get; set; }
    }
}