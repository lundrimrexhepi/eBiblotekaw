using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eBibloteka.Models
{
    public class QasjaModel
    {
        [Required(ErrorMessage = "Ju lutem shënoni përdoruesin!")]/*(ErrorMessageResourceName ="PlotesoPerdoruesin" , ErrorMessageResourceType =typeof(Resources.Resource))]*/
        public string Perdoruesi { get; set; }

        [Required(ErrorMessage = "Ju lutem shënoni fjalëkalimin!")]/*(ErrorMessageResourceName = "PlotesoFjalekalimin", ErrorMessageResourceType = typeof(Resources.Resource))]*/
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ErrorMsg { get; set; }
    }
}