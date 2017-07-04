using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using Xamarin.Forms;
using app_pesquisa.Droid.sqlite;
using app_pesquisa.interfaces;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace app_pesquisa.Droid.sqlite
{
    public class SQLiteAndroid : ISQLite
    {
        public SQLiteAndroid() { }
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "dbPesquisa.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); 
            var path = Path.Combine(documentsPath, sqliteFilename);
            
            var conn = new SQLiteConnection(path);
                        
            return conn;
        }
    }
}