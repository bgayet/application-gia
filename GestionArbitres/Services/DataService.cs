using GestionArbitres.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionArbitres.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetTableau(Action<Tableau, Exception> callback)
        {
            Tableau result = new Tableau();
            List<Partie> parties;
            List<ParamTableauParties> paramTableauPartie;

            try
            {
                using (var db = new SQLiteConnection(SQLiteDatabase.DataBasePath))
                {
                    paramTableauPartie = db.Table<ParamTableauParties>().ToList();
                }

                parties = paramTableauPartie
                    .Select(param => new Partie()
                    {
                        Numero = param.NumPartie,
                        NumeroPhase = param.NumPhase,
                        PartieVainqueur = new Partie() { Numero = param.NumPartieVainqueur },
                        PartiePerdant = new Partie() { Numero = param.NumPartiePerdant },
                        Position = param.Position,
                        Statut = StatutPartie.ALancer
                    }).OrderBy(x => x.Position).ToList();

                parties.ForEach(partie =>
                {
                    partie.PartieVainqueur = parties.Find(x => x.Numero == partie.PartieVainqueur?.Numero);
                    partie.PartiePerdant = parties.Find(x => x.Numero == partie.PartieVainqueur?.Numero);
                });

                result.Parties = parties.ToArray();
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }

            callback(result, null);
        }

    }
}