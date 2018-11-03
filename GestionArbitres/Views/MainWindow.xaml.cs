using BGayet.GIA.Models;
using BGayet.GIA.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BGayet.GIA.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }

    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HeaderLstArbitresTemplate { get; set; }
        public DataTemplate HeaderClassementJoueursTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ContentPresenter cp = container as ContentPresenter;

            if (cp != null)
            {
                CollectionViewGroup cvg = cp.Content as CollectionViewGroup;

                if (cvg.Items.Count > 0)
                {
                    Joueur joueur = cvg.Items[0] as Joueur;
                    if (joueur != null)
                        return HeaderClassementJoueursTemplate;
                    else
                        return HeaderLstArbitresTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}