using BGayet.GIA.Models;
using System;

namespace BGayet.GIA.Services
{
    public interface IDataService
    {
        //void GetData(Action<DataItem, Exception> callback);
        void GetTableau(Action<Tableau, Exception> callback);
    }
}
