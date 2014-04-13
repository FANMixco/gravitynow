using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gNowWP.ViewModels
{
    class gravityData
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]

        public int idgravity { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }
        public double gravity { get; set; }
        public string status { get; set; }
        public DateTime registered { get; set; }
    }

    public class sqliteDB
    {
        private string dbPath;
        public SQLiteConnection db;

        public sqliteDB()
        {
            this.dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
        }

        public void close()
        {
            db.Close();
        }

        public void createDB()
        {
            if (!FileExists("db.sqlite").Result)
            {
                open();
                using (this.db)
                {
                    this.db.CreateTable<gravityData>();
                }
            }
        }

        public void open()
        {
            this.db = new SQLiteConnection(dbPath);
        }

        private async Task<bool> FileExists(string fileName)
        {
            var result = false;
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                result = true;
            }
            catch { }

            return result;
        }
    }
}
