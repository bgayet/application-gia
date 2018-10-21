using GestionArbitres.Models;
using System;

namespace GestionArbitres.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        void GetTableau(Action<Tableau, Exception> callback);
    }
}
