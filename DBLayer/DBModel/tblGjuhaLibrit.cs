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
    
    public partial class tblGjuhaLibrit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblGjuhaLibrit()
        {
            this.tblLibri = new HashSet<tblLibri>();
        }
    
        public int GjuhaLibritID { get; set; }
        public string Pershkrimi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLibri> tblLibri { get; set; }
    }
}
