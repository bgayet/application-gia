using BGayet.GIA.Database;
using BGayet.GIA.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BGayet.GIA.Services
{

    public class DataService : IDataService
    {

        public void GeParamTableauById(int id, Action<ParamTableau, Exception> callback)
        {
            try
            {
                using (var db = new SQLiteConnection(SQLiteDatabase.DataBasePath))
                    callback(db.GetWithChildren<ParamTableau>(id), null);
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }


        public void GeTableauById(Action<Tableau, Exception> callback, int id)
        {
            Tableau result = new Tableau();
            List<Partie> parties;

            try
            {
                using (var db = new SQLiteConnection(SQLiteDatabase.DataBasePath))
                {

                    parties = db.GetWithChildren<ParamTableau>(id)
                        .ParamTableauParties
                        .OrderBy(x => x.Position)
                        .Select(param => new Partie()
                        {
                            Numero = param.NumPartie,
                            NumeroPhase = param.NumPhase,
                            PartieVainqueur = new Partie() { Numero = param.NumPartieVainqueur },
                            PartiePerdant = new Partie() { Numero = param.NumPartiePerdant },
                            Statut = StatutPartie.ALancer
                        }).ToList();

                    parties.ForEach(partie =>
                    {
                        partie.PartieVainqueur = parties.Find(x => x.Numero == partie.PartieVainqueur?.Numero);
                        partie.PartiePerdant = parties.Find(x => x.Numero == partie.PartieVainqueur?.Numero);
                    });
                }

                result.Parties = parties;
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }

            callback(result, null);
        }

    }
}