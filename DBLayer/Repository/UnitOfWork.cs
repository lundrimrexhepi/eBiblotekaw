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

        public GenericRepository<tblPerdoruesit> DepartmentRepository
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

        public GenericRepository<tblLibri> CourseRepository
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