using System;
using BGayet.GIA.Models;
using BGayet.GIA.Services;

namespace BGayet.GIA.Design
{
    public class DesignDataService : IDataService
    {
        //public void GetData(Action<DataItem, Exception> callback)
        //{
        //    // Use this to create design time data
        //    var item = new DataItem("Welcome to MVVM Light [design]");
        //    callback(item, null);
        //}

        public void GetTableau(Action<Tableau, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}