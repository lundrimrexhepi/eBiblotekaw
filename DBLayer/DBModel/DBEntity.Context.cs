﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BiblotekaEntities : DbContext
    {
        public BiblotekaEntities()
            : base("name=BiblotekaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblAutori> tblAutori { get; set; }
        public virtual DbSet<tblDhoma> tblDhoma { get; set; }
        public virtual DbSet<tblGjuhaLibrit> tblGjuhaLibrit { get; set; }
        public virtual DbSet<tblGrupi> tblGrupi { get; set; }
        public virtual DbSet<tblHistoriatet> tblHistoriatet { get; set; }
        public virtual DbSet<tblHistoriatetLlojet> tblHistoriatetLlojet { get; set; }
        public virtual DbSet<tblHuazimi> tblHuazimi { get; set; }
        public virtual DbSet<tblKomunat> tblKomunat { get; set; }
        public virtual DbSet<tblKthimi> tblKthimi { get; set; }
        public virtual DbSet<tblLibri> tblLibri { get; set; }
        public virtual DbSet<tblPerdoruesit> tblPerdoruesit { get; set; }
        public virtual DbSet<tblRafti> tblRafti { get; set; }
    }
}