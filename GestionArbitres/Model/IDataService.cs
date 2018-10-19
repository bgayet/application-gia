using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionArbitres.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
