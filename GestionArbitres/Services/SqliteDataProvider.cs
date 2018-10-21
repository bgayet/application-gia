using System;
using System.IO;
using SQLite;
using GestionArbitres.Models;

namespace GestionArbitres.Services
{
    public class SqliteDataProvider
    {
        #region Constants

        private const string FileName = "GestionArbitresDB.db";

        #endregion Constants

        #region Constructors

//        public SqliteDataProvider()
//        {
//            DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

//            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
//            builder.DataSource = DataPath;
//            ConnectionString = builder.ToString();

//            if (!File.Exists(DataPath))
//            {
//                SQLiteConnection.CreateFile(DataPath);

//                string sqlCreateTable = @"CREATE TABLE Friend(
//Id varchar(40) Primary Key,
//Name varchar(20) Not Null,
//Email varchar(20),
//IsDeveloper bool,
//BirthDate date)";

//                ExecuteNonQuery(sqlCreateTable);
//            }
//        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The full data path of the sqlite database file
        /// </summary>
        public string DataPath { get; private set; }

        /// <summary>
        /// The connection string
        /// </summary>
        protected string ConnectionString { get; set; }

        #endregion Properties

        #region Methods

        public static void AddStock(SQLiteConnection db, string symbol)
        {
            var s = db.Insert(new Tableau());
        }

        #endregion Methods
    }
}
