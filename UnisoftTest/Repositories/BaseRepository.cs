
using Microsoft.Maui.Controls;
using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnisoftTest.MVVM.Models;
using UniTest.MVVM.Models;
using UniToolbox.MVVM.Models;
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
        public AppSettings AdministratorSet { get; set; }

        public List<Modules> AllModules { get; set; }

        public BaseRepository()
        {
            connection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);

            connection.CreateTable<AutoItScript>();
            connection.CreateTable<AppSettings>();
            connection.CreateTable<CopyBaseScripts>();
            connection.CreateTable<Modules>();
            connection.CreateTable<CustomScripts>();

            AllModules = GetAllModules();

            AddModules();

        }

        #region Module
        public void AddModules()
        {
            var newModules = new List<Modules>
                            {
                                new Modules { ModulID = 0, ModuleName = "PanelAdministracyjny", ModuleAccess = false, LastModified = DateTime.Now, ImgVisualState = "fav.png" },
                                new Modules { ModulID = 1, ModuleName = "TestyWydajnosciowe", ModuleAccess = false, LastModified = DateTime.Now, ImgVisualState = "fav.png" },
                                new Modules { ModulID = 2, ModuleName = "KopiowanieBazy", ModuleAccess = false, LastModified = DateTime.Now, ImgVisualState = "fav.png" },
                                new Modules { ModulID = 3, ModuleName = "DodatkoweSkrypty", ModuleAccess = false, LastModified = DateTime.Now, ImgVisualState = "fav.png" }
                            };
            if (AllModules.Count  != newModules.Count)
            {
                foreach (var module in newModules)
                {
                    if (!AllModules.Any(m => m.ModuleName == module.ModuleName)) // Sprawdź, czy dany moduł już istnieje
                    {
                        connection.Insert(module);
                    }
                }
            }
            //Modules AdministratorPage = new Modules
            //{
            //    ModulID = 0,
            //    ModuleName = "PanelAdministracyjny",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.Insert(AdministratorPage);

            //Modules ResultPage = new Modules
            //{
            //    ModulID = 1,
            //    ModuleName = "TestyWydajnosciowe",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.Insert(ResultPage);

            //Modules CopyBasePage = new Modules
            //{
            //    ModulID = 2,
            //    ModuleName = "KopiowanieBazy",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.Insert(CopyBasePage);
            //Modules CustomScriptsPage = new Modules
            //{
            //    ModulID = 3,
            //    ModuleName = "DodatkoweSkrypty",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.Insert(CopyBasePage);
        }
        public void AddOrUpdateModule(Modules module)
        {
            int result = 0;
            var existingModule = connection.Find<Modules>(module.ModulID);
            try
            {

                //if (script.BaseScriptId != 0)
                //if (connection.Query<CopyBaseScripts>($"SELECT * FROM CopyBaseScripts WHERE BaseScriptId={script.BaseScriptId}").Any())
                if (existingModule != null)
                {
                    module.LastModified = DateTime.Now;
                    result = connection.Update(module);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    module.LastModified = DateTime.Now;
                    result = connection.Insert(module);
                    StatusMessage = $"{result} wiersz dodany!";
                }


            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            Console.WriteLine(StatusMessage);
        }

        public List<Modules> GetAllModules()
        {
            try
            {
                //return connection.Table<AutoItScript>().ToList();
                return connection.Query<Modules>("SELECT * FROM Modules").ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public void VisualModuleSTatus(Modules script)
        {
            connection.Update(script);

        }

        public void DeleteAllModules()
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                connection.Execute($"DELETE FROM Modules ");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            AddModules();
        }

        #endregion

        #region CustomScript
        public void AddOrUpdateCustomScript(CustomScripts script)
        {
            int result = 0;
            var existingScript = connection.Find<CustomScripts>(script.CustomScriptId);
            try
            {


                //if (script.BaseScriptId != 0)
                //if (connection.Query<CopyBaseScripts>($"SELECT * FROM CopyBaseScripts WHERE BaseScriptId={script.BaseScriptId}").Any())
                if (existingScript != null)
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

        public List<CustomScripts> GetAllCustomScripts()
        {
            try
            {
                //return connection.Table<AutoItScript>().ToList();
                return connection.Query<CustomScripts>("SELECT * FROM CustomScripts").ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public void DeleteCustomScript(int id)
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                connection.Execute($"DELETE FROM CustomScripts where CustomScriptId={id}");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        #endregion


        #region BaseScript
        public void AddOrUpdateBaseScript(CopyBaseScripts script)
        {
            int result = 0;
            var existingScript = connection.Find<CopyBaseScripts>(script.BaseScriptId);
            try
            {


                //if (script.BaseScriptId != 0)
                //if (connection.Query<CopyBaseScripts>($"SELECT * FROM CopyBaseScripts WHERE BaseScriptId={script.BaseScriptId}").Any())
                if (existingScript != null)
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

        public void AddOrUpdateAppAdministrator(bool admValue)
        {
            AdministratorSet = new AppSettings();
            int result = 0;
            AdministratorSet.SettingsName = "Administrator2";
            AdministratorSet.SettingsId = 1;
            if (admValue == true)
            {
                AdministratorSet.SettingsValue = "1";
            }
            else
            {
                AdministratorSet.SettingsValue = "0";
            }

            var existingScript = connection.Find<AppSettings>(AdministratorSet.SettingsId);


            try
            {
                if (existingScript != null)
                {
                    AdministratorSet.SettingsCreatedAt = DateTime.Now;
                    result = connection.Update(AdministratorSet);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    AdministratorSet.SettingsCreatedAt = DateTime.Now;
                    result = connection.Insert(AdministratorSet);
                    StatusMessage = $"{result} wiersz dodany!";
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void AddOrUpdateAppSettingsPathExe(AppSettings appSettings)
        {
            int result = 0;
            //appSettings.SettingsId = 0;

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

        public AppSettings GetSettings(int id)
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

        public AppSettings GetAdministratorStatus()
        {
            int id = 1;
            try
            {

                return connection.Table<AppSettings>().FirstOrDefault(x => x.SettingsId == id);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
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
