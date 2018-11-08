using BGayet.GIA.Models;
using System;
using System.Collections.Generic;

namespace BGayet.GIA.Services
{
    public interface IDataService
    {
        void GeParamTableauById(int id, Action<ParamTableau, Exception> callback);
        void GeTableauById(Action<Tableau, Exception> callback, int id);
        void GetAllParamTableaux(Action<List<ParamTableau>, Exception> callback);
    }
}