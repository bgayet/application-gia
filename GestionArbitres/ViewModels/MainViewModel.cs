using GalaSoft.MvvmLight;
using BGayet.GIA.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using BGayet.GIA.Services;
using BGayet.GIA.Utils;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.Win32;

namespace BGayet.GIA.ViewModels
{

    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        public ParamTableau _paramTableau;
        public Tableau Tableau { get; set; }
        public ParamTableau ParamTableau { get; set; }

        public List<JoueurModel> ListJoueurModel { get; set; } 


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            ListJoueurModel = new List<JoueurModel>();
            _dataService = dataService;
            InitializeTableau(1);
        }

        private void InitializeParties()
        {
            var result = _paramTableau.ParamTableauParties
                .OrderBy(x => x.Position)
                .Select(param => new Partie()
                {
                    Numero = param.NumPartie,
                    NumeroPhase = param.NumPhase,
                    PartieVainqueur = new Partie() { Numero = param.NumPartieVainqueur },
                    PartiePerdant = new Partie() { Numero = param.NumPartiePerdant },
                    Statut = StatutPartie.ALancer
                }).ToList();

            result.ForEach(partie =>
            {
                partie.PartieVainqueur = result.Find(x => x.Numero == partie.PartieVainqueur?.Numero);
                partie.PartiePerdant = result.Find(x => x.Numero == partie.PartieVainqueur?.Numero);
            });

            Tableau.Parties = result;
        }

        private void InitializeTables(DataTable dt)
        {
            var result = new List<Table>();

            foreach (var param in _paramTableau.ParamTableauJoueurs)
            {
                DataRow row = dt.Rows[param.NumLigneFichier - 2];

                string numTableStr = Convert.ToString(row[Constants.FichierEnteteNumTable]);
                if (!string.IsNullOrEmpty(numTableStr))
                {
                    int numTableInt = Convert.ToInt32(numTableStr);
                    if (numTableInt > 0)
                    {
                        Table table = new Table() { Numero = numTableInt };                     
                        result.Add(table);
                        Partie partie = Tableau.Parties.Find(x => x.Numero == param.NumPartieTableau);
                        partie.Table = table;
                    }
                }
            }

            Tableau.Tables = result;
        }

        private void InitializeJoueurs(DataTable dt)
        {
            var joueurs = new List<Joueur>();

            foreach (var param in _paramTableau.ParamTableauJoueurs)
            {
                DataRow row = dt.Rows[param.NumLigneFichier - 2];
                Partie partie = Tableau.Parties.Find(x => x.Numero == param.NumPartieTableau);

                if (param.ClassementJoueur1.HasValue)
                {
                    Joueur joueur1 = new Joueur
                    {
                        Numero = Convert.ToString(row[Constants.FichierEnteteDossardJ1]),
                        Nom = Convert.ToString(row[Constants.FichierEnteteNomJ1]),
                        Prenom = Convert.ToString(row[Constants.FichierEntetePrenomJ1])
                    };

                    if (joueur1.Nom.ToUpper() == Constants.NomJoueurAbsent)
                        joueur1.EstAbsent = true;

                    ListJoueurModel.Add(new JoueurModel()
                    {
                        Joueur = joueur1,
                        CompteurArbitre = 0,
                        Classement = param.ClassementJoueur1.Value,
                        NumGroupeArbitre = _paramTableau.ParamTableauGroupes.FirstOrDefault(x => x.ClassementJoueurs == param.ClassementJoueur1.Value).NumGroupe
                    });

                    partie.Joueur1 = joueur1;
                    joueurs.Add(joueur1);
                }

                if (param.ClassementJoueur2.HasValue)
                {
                    Joueur joueur2 = new Joueur
                    {
                        Numero = Convert.ToString(row[Constants.FichierEnteteDossardJ2]),
                        Nom = Convert.ToString(row[Constants.FichierEnteteNomJ2]),
                        Prenom = Convert.ToString(row[Constants.FichierEntetePrenomJ2])
                    };

                    if (joueur2.Nom.ToUpper() == Constants.NomJoueurAbsent)
                        joueur2.EstAbsent = true;

                    ListJoueurModel.Add(new JoueurModel()
                    {
                        Joueur = joueur2,
                        CompteurArbitre = 0,
                        Classement = param.ClassementJoueur2.Value,
                        NumGroupeArbitre = _paramTableau.ParamTableauGroupes.FirstOrDefault(x => x.ClassementJoueurs == param.ClassementJoueur2.Value).NumGroupe
                    });

                    partie.Joueur2 = joueur2;
                    joueurs.Add(joueur2);
                }
            }

            Tableau.Joueurs = joueurs;
        }

        private void InitializeTableau(int id)
        {
            Tableau = new Tableau();

            // Sélection du fichier
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Constants.FichierExcelFilter;
            if (openFileDialog.ShowDialog() == false)
                return;

            // Lecture fichier dans Datatable
            DataTable dataTable = ExcelSheetHelper.ReadAsDataTable(openFileDialog.FileName);

            // Récupération du paramétrage
            _dataService.GeParamTableauById(id, (item, error) =>
            {
                //ManageError(error);
                _paramTableau = item;
            });

            // Initialisation Parties/Joueurs/Tables
            InitializeParties();
            InitializeJoueurs(dataTable);
            InitializeTables(dataTable);
            //InitializeListView();
        }

        private void LancerPartie(Partie partie)
        {
            partie.Statut = StatutPartie.EnCours;
        }

        private void ArreterPartie(Partie partie)
        {
            partie.Statut = StatutPartie.Terminee;
            //partie.Joueur1.Statut = StatutJoueur.Libre;
            //partie.Joueur2.Statut = StatutJoueur.Libre;
            //partie.Arbitre.Statut = StatutJoueur.Libre;
        }

        private void ManageError(Exception error)
        {
            throw new NotImplementedException();
        }

        private void InitializeAutre()
        { 


            int index = 0;
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    
                    Rectangle rect = new Rectangle();

                    Setter set = new Setter(Shape.FillProperty, Brushes.Beige);

                    Binding bindingTagProperty = new Binding(string.Format("Parties[{0}]", index));
                    rect.SetBinding(FrameworkElement.TagProperty, bindingTagProperty);


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