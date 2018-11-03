﻿using GalaSoft.MvvmLight;
using BGayet.GIA.Models;
using System.Windows.Data;
using BGayet.GIA.Services;
using BGayet.GIA.Utils;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BGayet.GIA.ViewModels
{

    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private ParamTableau _paramTableau;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            InitializeTableau(1);
            DemarrerPartieCommand = new RelayCommand<Partie>(DemarrerPartie);
            ArreterPartieCommand = new RelayCommand<Joueur>(ArreterPartie);
        }

        public ParamTableau ParamTableau { get; set; }
        public Tableau Tableau { get; set; }

        public RelayCommand<Partie> DemarrerPartieCommand { get; set; }
        public RelayCommand<Joueur> ArreterPartieCommand { get; set; }

        private void InitializeParties()
        {
            var result = _paramTableau.ParamTableauParties
                .OrderBy(x => x.Position)
                .Select(param => new Partie()
                {
                    Numero = param.NumPartie,
                    Statut = StatutPartie.ALancer
                }).ToList();

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
            var joueurs = new ObservableCollection<Joueur>();

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
                        Prenom = Convert.ToString(row[Constants.FichierEntetePrenomJ1]),
                        Classement = param.ClassementJoueur1.Value,
                        CompteurArbitre = 0,
                        NumGroupeArbitre = _paramTableau.ParamTableauGroupes.FirstOrDefault(x => x.ClassementJoueurs == param.ClassementJoueur1.Value).NumGroupe
                    };

                    if (joueur1.Nom.ToUpper() == Constants.NomJoueurAbsent)
                        joueur1.EstAbsent = true;

                    partie.Joueur1 = joueur1;
                    joueurs.Add(joueur1);
                }

                if (param.ClassementJoueur2.HasValue)
                {
                    Joueur joueur2 = new Joueur
                    {
                        Numero = Convert.ToString(row[Constants.FichierEnteteDossardJ2]),
                        Nom = Convert.ToString(row[Constants.FichierEnteteNomJ2]),
                        Prenom = Convert.ToString(row[Constants.FichierEntetePrenomJ2]),
                        Classement = param.ClassementJoueur2.Value,
                        CompteurArbitre = 0,
                        NumGroupeArbitre = _paramTableau.ParamTableauGroupes.FirstOrDefault(x => x.ClassementJoueurs == param.ClassementJoueur2.Value).NumGroupe
                    };

                    if (joueur2.Nom.ToUpper() == Constants.NomJoueurAbsent)
                        joueur2.EstAbsent = true;

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
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = Constants.FichierExcelFilter };
            if (openFileDialog.ShowDialog() == false)
                return;

            // Lecture fichier dans Datatable
            DataTable dataTable = ExcelSheetHelper.ReadAsDataTable(openFileDialog.FileName);
            //DataTable dataTable = null;
            // Récupération du paramétrage
            _dataService.GeParamTableauById(id, (item, error) =>
            {
                //ManageError(error);
                _paramTableau = item;
            });
            ParamTableau = _paramTableau;
            // Initialisation Parties/Joueurs/Tables
            InitializeParties();
            InitializeJoueurs(dataTable);
            InitializeTables(dataTable);
            InitializeStatutParties();
        }

        private void InitializeStatutParties()
        {
            Tableau.Parties.ForEach(partie =>
            {
                if (partie.Joueur1 != null && partie.Joueur1.Statut == StatutJoueur.Libre 
                &&  partie.Joueur2 != null && partie.Joueur2.Statut == StatutJoueur.Libre)
                {
                    partie.Joueur1.Statut = StatutJoueur.Occupe;
                    partie.Joueur2.Statut = StatutJoueur.Occupe;
                }
            });

            Tableau.Parties.ForEach(partie =>
            {
                if (partie.Joueur1 != null && partie.Joueur1.Statut == StatutJoueur.Occupe
                && partie.Joueur2 != null && partie.Joueur2.Statut == StatutJoueur.Occupe)
                {
                    partie.Arbitre = FindArbitre(partie);
                    partie.Arbitre.Statut = StatutJoueur.Occupe;
                    partie.Arbitre.CompteurArbitre += 1;
                    partie.Table = FindTable();
                    partie.Table.Statut = StatutTable.Occupe;
                    partie.Statut = StatutPartie.EnAttente;
                }
            });
        }

        private Joueur FindArbitre(Partie partie)
        {
            var paramPartie = _paramTableau.ParamTableauParties
                .Find(param => param.NumPartie == partie.Numero);

            var joueursPrioritaires = _paramTableau.ParamTableauParties
                .Where(param => param.NumPhase == paramPartie.NumPhase + 1)
                .Select(param => Tableau.Parties.Find(p => p.Numero == param.NumPartie))
                .Where(p =>
                {
                    return p.Statut == StatutPartie.ALancer &&
                    p.Joueur1 != null && p.Joueur2 != null &&
                    (p.Joueur1.Statut == StatutJoueur.Occupe || p.Joueur1.EstAbsent || p.Joueur1.EstForfait || p.Joueur2.Statut == StatutJoueur.Occupe || p.Joueur2.EstAbsent || p.Joueur2.EstForfait) &&
                    (p.Joueur1.PeutArbitrer || p.Joueur2.PeutArbitrer);
                })
                .Select(p => p.Joueur1.PeutArbitrer ? p.Joueur1 : p.Joueur2)
                .OrderBy(j => j.CompteurArbitre);

            if (joueursPrioritaires.Count() > 0)
                return joueursPrioritaires.First();
            else
                return Tableau.Joueurs
                    .Where(joueur =>  joueur.NumGroupeArbitre == paramPartie.NumGroupeArbitre && joueur.PeutArbitrer)
                    .OrderBy(joueur => joueur.CompteurArbitre)
                    .FirstOrDefault();
        }

        private Table FindTable()
        {
            return Tableau.Tables
                .Where(table => table.Statut == StatutTable.Libre)
                .FirstOrDefault();
        }

        private void DemarrerPartie(Partie partie)
        {
            if(partie.Statut == StatutPartie.EnAttente)
                partie.Statut = StatutPartie.EnCours;
        }

        private void ArreterPartie(Joueur vainqueur)
        {
            var partie = Tableau.Parties.Find(x => x.Statut == StatutPartie.EnCours && (x.Joueur1 == vainqueur || x.Joueur2 == vainqueur));
            Joueur joueurP;
            if (partie.Joueur1 == vainqueur)
            {
                partie.Resultat = ResultatPartie.Joueur1Vainqueur;
                joueurP = partie.Joueur2;
            }
            else
            {
                partie.Resultat = ResultatPartie.Joueur2Vainqueur;
                joueurP = partie.Joueur1;
            }

            MajListeTableau16(partie);

            partie.Statut = StatutPartie.Terminee;
            partie.Joueur1.Statut = StatutJoueur.Libre;
            partie.Joueur2.Statut = StatutJoueur.Libre;
            partie.Arbitre.Statut = StatutJoueur.Libre;
            partie.Table.Statut = StatutTable.Libre;

            var paramPartie = _paramTableau.ParamTableauParties.Find(x => x.NumPartie == partie.Numero);
            var partieV = Tableau.Parties.FirstOrDefault(x => x.Numero == paramPartie.NumPartieVainqueur);
            var partieP = Tableau.Parties.FirstOrDefault(x => x.Numero == paramPartie.NumPartiePerdant);

            if (partieV != null)
            {
                if (partieV.Joueur1 == null)
                    partieV.Joueur1 = vainqueur;
                else
                    partieV.Joueur2 = vainqueur;
            }

            if (partieP != null)
            { 
                if (partieP.Joueur1 == null)
                    partieP.Joueur1 = joueurP;
                else
                    partieP.Joueur2 = joueurP;
            }

            var numPhase = _paramTableau.ParamTableauParties.Find(x => x.NumPartie == partie.Numero).NumPhase;
            var numPartieLst = _paramTableau.ParamTableauParties.Where(x => x.NumPhase == numPhase + 1).Select(x => x.NumPartie);

            foreach(var p in Tableau.Parties.Where(p => numPartieLst.Contains(p.Numero) && p.Statut == StatutPartie.ALancer)) 
            {
                if (p.Joueur1 != null && p.Joueur1.Statut == StatutJoueur.Libre
                 && p.Joueur2 != null && p.Joueur2.Statut == StatutJoueur.Libre
                 && FindArbitre(p) != null && FindTable() != null)
                {
                    p.Joueur1.Statut = StatutJoueur.Occupe;
                    p.Joueur2.Statut = StatutJoueur.Occupe;
                    p.Arbitre = FindArbitre(p);
                    p.Arbitre.Statut = StatutJoueur.Occupe;
                    p.Arbitre.CompteurArbitre += 1;
                    p.Table = FindTable();
                    p.Table.Statut = StatutTable.Occupe;
                    p.Statut = StatutPartie.EnAttente;
                }
            }
        }

        private void MajListeTableau16(Partie partie)
        {
            if (new List<int>() { 1, 2, 3, 4 }.Contains(partie.Numero))
            {
                Joueur v;
                Joueur p;
                if (partie.Resultat == ResultatPartie.Joueur1Vainqueur)
                {
                    v = partie.Joueur1;
                    p = partie.Joueur2;
                }
                else
                {
                    v = partie.Joueur2;
                    p = partie.Joueur1;
                }

                //  Remove and Re-Add - permet de rafraichir la vue
                Tableau.Joueurs.Remove(v);
                Tableau.Joueurs.Remove(p);

                v.Classement = 2;
                v.NumGroupeArbitre = 2;
                p.Classement = 3;
                p.NumGroupeArbitre = 1;

                Tableau.Joueurs.Add(v);
                Tableau.Joueurs.Add(p);
            }
        }

    }
}