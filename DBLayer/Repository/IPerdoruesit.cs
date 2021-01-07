using DBLayer.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repository
{
    interface IPerdoruesit:IDisposable
    {
        IEnumerable<tblPerdoruesit> GetUsers();
        tblPerdoruesit GetUserByID(int studentId);
        void InsertUser(tblPerdoruesit student);
        void DeleteUser(int PerdoruesiID);
        void UpdateUser(tblPerdoruesit Perdoruesit);
        void Save();
    }
}
