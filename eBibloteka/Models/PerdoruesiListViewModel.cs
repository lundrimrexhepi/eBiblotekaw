using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class PerdoruesiModel
    {
        public int perdoruesiID { get; set; }

        [Required(ErrorMessage = "Shkruani perdoruesin!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Shkruani emrin!")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Shkruani mbiemrin!")]
        public string Mbiemri { get; set; }

        [Required(ErrorMessage = "Shkruani numrin personal!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Lejohen vetem numrat!")]
        public string NumriPersonal { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Lejohen vetem numrat!")]
        public string Telefoni { get; set; }

        [EmailAddress(ErrorMessage = "Invalid E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Shkruaj fjalekalimin!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Fjalekalimi duhet te kete shkronje te madhe, shkronje te vogel, numer dhe 8 e me shume karaktere!")]
        public string Fjalekalimi { get; set; }

        [Required(ErrorMessage = "Zgjedh grupin!")]
        public long[] grupiID { get; set; }

        [Required(ErrorMessage = "Zgjedh komunat!")]
        public long[] komunaID { get; set; }

        public bool allKomunat { get; set; }

        public bool Aktive { get; set; }
    }
}