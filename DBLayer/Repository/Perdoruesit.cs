using DBLayer.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DBLayer.Repository
{
    public class Perdoruesit : IPerdoruesit, IDisposable
    {
        private BiblotekaEntities context;

        public Perdoruesit(BiblotekaEntities context)
        {
            this.context = context;
        }

        public void DeleteUser(int PerdoruesiID)
        {
            tblPerdoruesit User = context.tblPerdoruesit.Find(PerdoruesiID);
            context.tblPerdoruesit.Remove(User);
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

        public tblPerdoruesit GetUserByID(int UserID)
        {
            return context.tblPerdoruesit.Find(UserID);
        }

        public IEnumerable<tblPerdoruesit> GetUsers()
        {
            return context.tblPerdoruesit.ToList();
        }

        public void InsertUser(tblPerdoruesit student)
        {
            context.tblPerdoruesit.Add(student);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateUser(tblPerdoruesit Perdoruesit)
        {
            context.Entry(Perdoruesit).State = EntityState.Modified;
        }
    }
}