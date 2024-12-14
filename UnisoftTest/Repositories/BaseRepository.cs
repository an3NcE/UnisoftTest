using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.MVVM.Models;

namespace UnisoftTest.Repositories
{
    [AddINotifyPropertyChangedInterface]
    public class BaseRepository
    {
        SQLiteConnection connection;
        public string StatusMessage;

        public BaseRepository()
        {
            connection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);

            connection.CreateTable<AutoItScript>();


        }

        public void AddOrUpdate(AutoItScript script)
        {
            int result = 0;
            try
            {
                if(script.ScriptId != 0)
                {
                    result = connection.Update(script);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    result = connection.Insert(script);
                    StatusMessage = $"{result} wiersz dodany!";
                }
                

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
                //return connection.Table<AutoItScript>().ToList();
                return connection.Query<AutoItScript>("SELECT * FROM AutoItScripts").ToList();
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

        public void Delete(int id)
        {
            try
            {
                var script = Get(id);
                connection.Delete(script);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }


    }
}
