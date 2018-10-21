using System;
using GestionArbitres.Model;
using GestionArbitres.Models;

namespace GestionArbitres.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data
            var item = new DataItem("Welcome to MVVM Light [design]");
            callback(item, null);
        }

        public void GetTableau(Action<Tableau, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}