using BGayet.GIA.Models;
using System;

namespace BGayet.GIA.Services
{
    public interface IDataService
    {
        void GeParamTableauById(int id, Action<ParamTableau, Exception> callback);
        void GeTableauById(Action<Tableau, Exception> callback, int id);
    }
}