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

            //InitializeAutre();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        //private void InitializeAutre()
        //{
        //    var objet = (MainViewModel) DataContext;
        //    int index = 0;
        //    for (int i = 1; i < 5; i++)
        //    {
        //        for (int j = 1; j < 8; j++)
        //        {

        //            Rectangle rect = new Rectangle();
        //            Setter set = new Setter(Shape.FillProperty, Brushes.Aqua);
        //            Binding bindingTagProperty = new Binding(string.Format("Tableau.Parties[{0}]", index));
        //            rect.SetBinding(FrameworkElement.TagProperty, bindingTagProperty);


        //            Style style = new Style(typeof(Rectangle));
        //            DataTrigger etatTrigger = new DataTrigger()
        //            {
        //                Binding = new Binding(string.Format("Tableau.Parties[{0}].Statut", index)),
        //                Value = StatutPartie.EnCours
        //            };

        //            DataTrigger etatTrigger1 = new DataTrigger()
        //            {
        //                Binding = new Binding(string.Format("Tableau.Parties[{0}].Statut", index)),
        //                Value = StatutPartie.Terminee
        //            };

        //            etatTrigger.Setters.Add(new Setter(Shape.FillProperty, Brushes.Blue));
        //            //etatTrigger.Setters.Add(new Setter(Shape.ForegroundProperty, new SolidColorBrush(Colors.Green)));
        //            etatTrigger1.Setters.Add(new Setter(Shape.FillProperty, Brushes.Red));

        //            style.Setters.Add(set);
        //            style.Triggers.Add(etatTrigger);
        //            style.Triggers.Add(etatTrigger1);
        //            rect.Style = style;

        //            Border border = new Border();
        //            border.Child = rect;
        //            Grid.SetRow(border, i);
        //            Grid.SetColumn(border, j);


        //            GridTableaux.Children.Add(border);


        //            //Button button = new Button();

        //            //button.Click += MakeButton;
        //            //button.Content = rencontre.Numero;

        //            //Binding bindingTagProperty = new Binding(string.Format("Rencontres[{0}]", index));
        //            //button.SetBinding(TagProperty, bindingTagProperty);

        //            //Binding bindingIsEnabledProperty = new Binding(string.Format("Rencontres[{0}].Etat", index));
        //            //button.SetBinding(IsEnabledProperty, bindingIsEnabledProperty);

        //            //Style style = new Style(typeof(Button));
        //            //DataTrigger etatTrigger = new DataTrigger()
        //            //{
        //            //    Binding = new Binding(string.Format("Rencontres[{0}].Etat", index)),
        //            //    Value = Rencontre.EtatEnum.EnCours
        //            //};

        //            //etatTrigger.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(System.Windows.Media.Colors.Green)));

        //            //style.Triggers.Add(etatTrigger);
        //            //button.Style = style;

        //            //border.Child = button;
        //            //Grid.SetRow(border, i);
        //            //Grid.SetColumn(border, j);
        //            //GridTableaux.Children.Add(border);

        //            index++;
        //        }
        //    }
        //}
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