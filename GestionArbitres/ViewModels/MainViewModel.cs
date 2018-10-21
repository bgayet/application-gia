using GalaSoft.MvvmLight;
using GestionArbitres.Model;
using GestionArbitres.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GestionArbitres.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            InitializeTableau();
            //_dataService.GetData(
            //    (item, error) =>
            //    {
            //        if (error != null)
            //        {
            //            // Report error here
            //            return;
            //        }

            //        WelcomeTitle = item.Title;
            //    });
        }

        private void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var partie = ((Rectangle)sender).Tag as Partie;
            partie.Statut = StatutPartie.Terminee;
        }

        private void OnMouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            var partie = ((Rectangle)sender).Tag as Partie;
            partie.Statut = StatutPartie.EnCours;
        }

        private void InitializeTableau()
        {
            Tableau tableau;
            _dataService.GetTableau(
                (item, error) =>
                {
                   if (error != null)
                   {
                        // Report error here
                        return;
                   }
                   tableau = item;
                });

            int index = 0;
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    
                    Rectangle rect = new Rectangle();

                    Setter set = new Setter(Shape.FillProperty, Brushes.Beige);

                    Binding bindingTagProperty = new Binding(string.Format("Parties[{0}]", index));
                    rect.SetBinding(FrameworkElement.TagProperty, bindingTagProperty);

                    rect.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    rect.MouseRightButtonDown += OnMouseRightButtonDown;

                    Style style = new Style(typeof(Rectangle));
                    DataTrigger etatTrigger = new DataTrigger()
                    {
                        Binding = new Binding(string.Format("Parties[{0}].Statut", index)),
                        Value = StatutPartie.EnCours
                    };

                    DataTrigger etatTrigger1 = new DataTrigger()
                    {
                        Binding = new Binding(string.Format("Parties[{0}].Statut", index)),
                        Value = StatutPartie.Terminee
                    };

                    etatTrigger.Setters.Add(new Setter(Shape.FillProperty, Brushes.Blue));
                    //etatTrigger.Setters.Add(new Setter(Shape.ForegroundProperty, new SolidColorBrush(Colors.Green)));
                    etatTrigger1.Setters.Add(new Setter(Shape.FillProperty, Brushes.Red));

                    style.Setters.Add(set);
                    style.Triggers.Add(etatTrigger);
                    style.Triggers.Add(etatTrigger1);
                    rect.Style = style;

                    Border border = new Border();
                    border.Child = rect;
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);


                    //GridTableaux.Children.Add(border);


                    //Button button = new Button();

                    //button.Click += MakeButton;
                    //button.Content = rencontre.Numero;

                    //Binding bindingTagProperty = new Binding(string.Format("Rencontres[{0}]", index));
                    //button.SetBinding(TagProperty, bindingTagProperty);

                    //Binding bindingIsEnabledProperty = new Binding(string.Format("Rencontres[{0}].Etat", index));
                    //button.SetBinding(IsEnabledProperty, bindingIsEnabledProperty);

                    //Style style = new Style(typeof(Button));
                    //DataTrigger etatTrigger = new DataTrigger()
                    //{
                    //    Binding = new Binding(string.Format("Rencontres[{0}].Etat", index)),
                    //    Value = Rencontre.EtatEnum.EnCours
                    //};

                    //etatTrigger.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(System.Windows.Media.Colors.Green)));

                    //style.Triggers.Add(etatTrigger);
                    //button.Style = style;

                    //border.Child = button;
                    //Grid.SetRow(border, i);
                    //Grid.SetColumn(border, j);
                    //GridTableaux.Children.Add(border);

                    index++;
                }
            }
        }

    }
}