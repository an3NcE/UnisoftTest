using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.MVVM.Models;

namespace UnisoftTest.Repositories
{
    public class BaseRepository
    {
        SQLiteConnection connection;
        public string StatusMessage;

        public BaseRepository()
        {
            connection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);

            connection.CreateTable<AutoItScript>();


        }

        public void Add(AutoItScript newScript)
        {
            int result = 0;
            try
            {
                result = connection.Insert(newScript);
                StatusMessage = $"{result} wiersz dodany!";

            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            Console.WriteLine(StatusMessage);
        }

        public List<AutoItScript> GetAll()
        {
            try
            {
                return connection.Table<AutoItScript>().ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public AutoItScript Get(int id)
        {
            try
            {
                return connection.Table<AutoItScript>().FirstOrDefault(x => x.ScriptId == id);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }


    }
}
