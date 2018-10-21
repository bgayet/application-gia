using GestionArbitres.Models;
using SQLite;
using System;
using System.IO;

namespace GestionArbitres
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
            int upgradedVersion = migrator.Update();
            if (upgradedVersion == 1)
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
        public int Update() => Update(targetMigration: null);

        /// <summary>
        /// Updates the database to a given migration.
        /// </summary>
        /// <param name="targetMigration">The migration to upgrade</param>
        public int Update(int? targetMigration)
        {
            if (!targetMigration.HasValue)
                targetMigration = int.MaxValue;

            using (var db = new SQLiteConnection(_dataBasePath))
            {
                string currentVersionStr = db.ExecuteScalar<string>(PragmaUserVersion);
                int currentVersion = Convert.ToInt32(currentVersionStr);
                int upgradedVersion = 0;

                // V0 -> V1
                if (currentVersion == 0 &&
                    targetMigration > currentVersion)
                {
                    db.CreateTable<ParamTableauParties>();

                    upgradedVersion = 1;
                    currentVersion = upgradedVersion;
                    db.ExecuteScalar<int>(string.Format("{0} = {1}", PragmaUserVersion, upgradedVersion));
                }

                return upgradedVersion;

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

