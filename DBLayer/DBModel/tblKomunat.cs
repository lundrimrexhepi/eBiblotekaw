//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBLayer.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblKomunat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblKomunat()
        {
            this.tblPerdoruesit = new HashSet<tblPerdoruesit>();
        }
    
        public int KomunaID { get; set; }
        public string Pershkrimi_SQ { get; set; }
        public string Pershkrimi_EN { get; set; }
        public string Pershkrimi_SR { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPerdoruesit> tblPerdoruesit { get; set; }
    }
}
