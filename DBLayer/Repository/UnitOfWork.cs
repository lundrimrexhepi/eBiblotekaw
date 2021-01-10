using DBLayer.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBLayer.Repository
{
    public class UnitOfWork : IDisposable
    {
        private BiblotekaEntities context = new BiblotekaEntities();
        private GenericRepository<tblPerdoruesit> UserRepository;
        private GenericRepository<tblLibri> BookRepository;
        private GenericRepository<tblAutori> AuthorRepository;
        private GenericRepository<tblGjuhaLibrit> bookLanguageRepository;
        private GenericRepository<tblHistoriatet> HistoryRepository;
        private GenericRepository<tblHuazimi> HuazimiRepository;
        private GenericRepository<tblKthimi> KthimiRepository;
        private GenericRepository<tblRafti> RaftiRepository;

        public GenericRepository<tblPerdoruesit> _UserRepository
        {
            get
            {

                if (this.UserRepository == null)
                {
                    this.UserRepository = new GenericRepository<tblPerdoruesit>(context);
                }
                return UserRepository;
            }
        }

        public GenericRepository<tblLibri> _BookRepository
        {
            get
            {

                if (this.BookRepository == null)
                {
                    this.BookRepository = new GenericRepository<tblLibri>(context);
                }
                return BookRepository;
            }
        }
        public GenericRepository<tblAutori> _AuthorRepository
        {
            get
            {

                if (this.AuthorRepository == null)
                {
                    this.AuthorRepository = new GenericRepository<tblAutori>(context);
                }
                return AuthorRepository;
            }
        }
        public GenericRepository<tblGjuhaLibrit> _bookLanguageRepository
        {
            get
            {

                if (this.bookLanguageRepository == null)
                {
                    this.bookLanguageRepository = new GenericRepository<tblGjuhaLibrit>(context);
                }
                return bookLanguageRepository;
            }
        }
        public GenericRepository<tblHistoriatet> _HistoryRepository
        {
            get
            {

                if (this.HistoryRepository == null)
                {
                    this.HistoryRepository = new GenericRepository<tblHistoriatet>(context);
                }
                return HistoryRepository;
            }
        }
        public GenericRepository<tblHuazimi> _HuazimiRepository
        {
            get
            {

                if (this.HuazimiRepository == null)
                {
                    this.HuazimiRepository = new GenericRepository<tblHuazimi>(context);
                }
                return HuazimiRepository;
            }
        }
        public GenericRepository<tblKthimi> _KthimiRepository
        {
            get
            {

                if (this.KthimiRepository == null)
                {
                    this.KthimiRepository = new GenericRepository<tblKthimi>(context);
                }
                return KthimiRepository;
            }
        }
        public GenericRepository<tblRafti> _RaftiRepository
        {
            get
            {

                if (this.RaftiRepository == null)
                {
                    this.RaftiRepository = new GenericRepository<tblRafti>(context);
                }
                return RaftiRepository;
            }
        }
        
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}