using System.Windows;
using BGayet.GIA.Database;
using GalaSoft.MvvmLight.Threading;

namespace BGayet.GIA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
            SQLiteDatabase.Initialize();
        }
    }
}