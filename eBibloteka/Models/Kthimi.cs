using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBibloteka.Models
{
    public class Kthimi
    {
        public string Libri { get; set; }
        public string Lexuesi { get; set; }
        public DateTime DataKthimit { get; set; }
        public bool? Demtuar { get; set; }
        public int KthimiID { get; set; }
    }
}