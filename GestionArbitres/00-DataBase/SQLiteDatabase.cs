using BGayet.GIA.Models;
using SQLite;
using System;
using System.IO;

namespace BGayet.GIA.Database
{

    public static class SQLiteDatabase
    {
        private const string DatabaseFileName = "GestionArbitresDB.db";
        private const string DataFileName = "data.sql";

        static SQLiteDatabase()
        {
            DataBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);
        }

        /// <summary>
        /// The full data path of the sqlite database file.
        /// </summary>
        public static string DataBasePath { get; private set; }

        /// <summary>
        /// Initialize the database.
        /// </summary>
        public static void Initialize()
        {
            SQLiteDbMigrator migrator = new SQLiteDbMigrator(DataBasePath);
            migrator.Update();
            PopulateDatabase();
        }

        /// <summary>
        /// Populate the database from sql script.
        /// </summary>
        public static void PopulateDatabase()
        {
            char separateur = ';';
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataFileName);
            string scriptSql = File.ReadAllText(dataPath);
            string[] lines = scriptSql
                .Replace(Environment.NewLine, string.Empty)
                .TrimEnd(separateur)
                .Split(separateur);

            using (var db = new SQLiteConnection(DataBasePath))
            {
                Array.ForEach(lines, x => db.Execute(x.Trim()));
            }
        }
    }

    public class SQLiteDbMigrator
    {
        private const string PragmaUserVersion = "PRAGMA user_version";
        private readonly string _dataBasePath;

        /// <summary>
        /// Initializes a new instance of the SQLiteDbMigrator class.
        /// </summary>
        /// <param name="dataBasePath">Specifies the path to the database file.</param>
        public SQLiteDbMigrator(string dataBasePath) => _dataBasePath = dataBasePath;

        /// <summary>
        /// Updates the database to the latest migration.
        /// </summary>
        public void Update() => Update(currentVersion: null, targetVersion: null);

        /// <summary>
        /// Updates the database to a given migration.
        /// </summary>
        /// <param name="currentVersion">The or version</param>
        /// <param name="targetVersion">The migration to upgrade</param>
        public void Update(int? currentVersion = null, int? targetVersion = null)
        {
            if (!targetVersion.HasValue)
                targetVersion = int.MaxValue;

            using (var db = new SQLiteConnection(_dataBasePath))
            {
                if (!currentVersion.HasValue)
                {
                    string currentVersionStr = db.ExecuteScalar<string>(PragmaUserVersion);
                    currentVersion = Convert.ToInt32(currentVersionStr);
                }

                // V0 -> V1
                if (currentVersion == 0 &&
                    targetVersion > currentVersion)
                {
                    db.CreateTable<ParamTableau>();

                    // l'ORM sqlite-net ne gère pas la création des clés étrangères
                    string queryCreateParamTableauParties =
                        @"CREATE TABLE [PARAM_TABLEAU_PARTIES] (
                                  [Id] INTEGER  NOT NULL
                                , [IdParamTableau] bigint  NULL
                                , [IdParamTableauListes] bigint  NULL
                                , [NumPartie] bigint  NULL
                                , [NumPhase] bigint  NULL
                                , [NumPartieVainqueur] bigint  NULL
                                , [NumPartiePerdant] bigint  NULL
                                , [Position] bigint  NULL
                                , FOREIGN KEY(IdParamTableau) REFERENCES PARAM_TABLEAU(Id)
                                , FOREIGN KEY(IdParamTableauListes) REFERENCES PARAM_TABLEAU_LISTES (Id)
                                , CONSTRAINT [sqlite_master_PK_PARAM_TABLEAU_PARTIES] PRIMARY KEY ([Id]));
                            CREATE INDEX [PARAM_TABLEAU_PARTIES_IdParamTableau] ON [PARAM_TABLEAU_PARTIES] ([IdParamTableau] ASC);
                            CREATE INDEX [PARAM_TABLEAU_PARTIES_IdParamTableauListes] ON [PARAM_TABLEAU_PARTIES] ([IdParamTableauListes] ASC);";

                    db.Execute(queryCreateParamTableauParties);

                    string queryCreateParamTableauJoueurs =
                        @"CREATE TABLE [PARAM_TABLEAU_JOUEURS] (
                                  [Id] INTEGER  NOT NULL
                                , [IdParamTableau] bigint  NULL
                                , [NumLigneFichier] bigint  NULL
                                , [NumPartieTableau] bigint  NULL
                                , [ClassementJoueur1] bigint  NULL
                                , [ClassementJoueur2] bigint  NULL
                                , FOREIGN KEY(IdParamTableau) REFERENCES PARAM_TABLEAU(Id)
                                , CONSTRAINT [sqlite_master_PK_PARAM_TABLEAU_JOUEURS] PRIMARY KEY ([Id]));
                            CREATE INDEX [PARAM_TABLEAU_JOUEURS_IdParamTableau] ON [PARAM_TABLEAU_JOUEURS] ([IdParamTableau] ASC);";

                    db.Execute(queryCreateParamTableauJoueurs);

                    string queryCreateParamTableauListes =
                            @"CREATE TABLE [PARAM_TABLEAU_LISTES] (
                                  [Id] INTEGER  NOT NULL
                                , [IdParamTableau] bigint  NULL
                                , [Classement] bigint  NULL
                                , [Nombre] bigint  NULL
                                , FOREIGN KEY(IdParamTableau) REFERENCES PARAM_TABLEAU(Id)
                                , CONSTRAINT [sqlite_master_PK_PARAM_TABLEAU_LISTES] PRIMARY KEY ([Id]));
                            CREATE INDEX [PARAM_TABLEAU_LISTES_IdParamTableau] ON [PARAM_TABLEAU_LISTES] ([IdParamTableau] ASC);";

                    db.Execute(queryCreateParamTableauListes);

                    currentVersion = 1;
                    db.ExecuteScalar<int>(string.Format("{0} = {1}", PragmaUserVersion, currentVersion));
                }

                // V1 -> V2
                //if (currentVersion == 1 &&
                //    targetMigration > currentVersion)
                //{
                //}

                //if (from == 1)
                //{
                //    connection.CreateTable<MyNewTable>();
                //    connection.CreateTable<Table1>(); // Màj via le mécanisme natif
                //    _dbVersion = 2;
                //    from = _dbVersion;
                //    connection.ExecuteScalar<int>(string.Format("PRAGMA user_version = {0}", dbVersion));
                //}
                //if (from == 2)
                //{
                //    // Màj "à la main"
                //    connection.ExecuteScalar<int>("ALTER TABLE Table1 ADD COLUMN NewColumn varchar(36);");
                //    connection.Execute("ALTER TABLE Table1 RENAME TO OLDTable1;");
                //}
            }
        }
    }

}

