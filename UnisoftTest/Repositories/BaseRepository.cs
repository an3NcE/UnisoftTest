using Microsoft.Maui.Controls;
using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.MVVM.Models;
using UniTest.MVVM.Models;
using static SQLite.SQLite3;

namespace UnisoftTest.Repositories
{
    [AddINotifyPropertyChangedInterface]
    public class BaseRepository
    {
        SQLiteConnection connection;
        public string StatusMessage;
        public List<AppSettings> AppSettingsList { get; set; }
        public AppSettings AppSettingsExePath { get; set; }


        public BaseRepository()
        {
            connection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);

            connection.CreateTable<AutoItScript>();
            connection.CreateTable<AppSettings>();
            connection.CreateTable<CopyBaseScripts>();


        }
        #region BaseScript
        public void AddOrUpdateBaseScript(CopyBaseScripts script)
        {
            int result = 0;
            try
            {
                

                if (script.BaseScriptId != 0)
                {
                    script.CreateScriptDate = DateTime.Now;
                    result = connection.Update(script);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    script.CreateScriptDate = DateTime.Now;
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

        public List<CopyBaseScripts> GetAllBaseScripts()
        {
            try
            {
                //return connection.Table<AutoItScript>().ToList();
                return connection.Query<CopyBaseScripts>("SELECT * FROM CopyBaseScripts").ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public void DeleteBaseScript(int id)
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                connection.Execute($"DELETE FROM CopyBaseScripts where BaseScriptId={id}");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }


        #endregion



        #region AppSettings

        public void AddOrUpdateAppSettingsPathExe(AppSettings appSettings)
        {
            int result = 0;
            try
            {
                if (appSettings.SettingsValue != null)
                {
                    appSettings.SettingsCreatedAt = DateTime.Now;
                    result = connection.Update(appSettings);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    appSettings.SettingsCreatedAt = DateTime.Now;
                    result = connection.Insert(appSettings);
                    StatusMessage = $"{result} wiersz dodany!";
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        public AppSettings GetPathExe(int id)
        {
            try
            {
                //connection.Execute($"DELETE FROM AppSettings where SettingsName<>{name}");
                return connection.Table<AppSettings>().FirstOrDefault(x => x.SettingsId == id);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public List<AppSettings> GetAllSettings()
        {
            try
            {
                //return connection.Table<AutoItScript>().ToList();
                return connection.Query<AppSettings>("SELECT * FROM AppSettings").ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public void DeleteSet()
        {
            try
            {
                //var script = Get(id);
                connection.Execute($"DELETE FROM AppSettings ");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        #endregion



        #region AutoItScript


        public void AddOrUpdate(AutoItScript script)
        {
            int result = 0;
            try
            {
                if (script.IsFavorite)
                {
                    
                    script.ImgFav = "unfav.png";
                }
                else
                {
                    
                    script.ImgFav = "fav.png";
                }

                if (script.ScriptId != 0)
                {
                    script.ScriptUpdatedAt = DateTime.Now;
                    result = connection.Update(script);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    script.ScriptCreatedAt = DateTime.Now;
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

        public List<AutoItScript> GetAllFav()
        {
            try
            {
                //return connection.Table<AutoItScript>().ToList();
                return connection.Query<AutoItScript>("SELECT * FROM AutoItScripts where IsFavorite=true").ToList();
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
                //var script = Get(id);
                //connection.Delete(script);
                connection.Execute($"DELETE FROM AutoItScripts where ScriptId={id}");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public void FavScript(AutoItScript script)
        {
            connection.Update(script);
            
        }

        #endregion
    }
}
