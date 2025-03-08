
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
        //SQLiteConnection connection;
        SQLiteAsyncConnection connection;
        public string StatusMessage;
        public List<AppSettings> AppSettingsList { get; set; }
        public AppSettings AppSettingsExePath { get; set; }
        public AppSettings AdministratorSet { get; set; }

        public List<Modules> AllModules { get; set; }



        public BaseRepository()
        {
            //connection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
            InitializeDatabaseAsync().ConfigureAwait(false);
            
        }

        private async Task InitializeDatabaseAsync()
        {
            string password = await Constants.GetDatabasePasswordAsync();
            var options = new SQLiteConnectionString(Constants.DatabasePath, true, password, postKeyAction: c =>
            c.Execute("PRAGMA cipher_compatibility = 3"));
            
            connection = new SQLiteAsyncConnection(options);
            


            await connection.CreateTableAsync<AutoItScript>();
            await connection.CreateTableAsync<AppSettings>();
            await connection.CreateTableAsync<CopyBaseScripts>();
            await connection.CreateTableAsync<Modules>();
            await connection.CreateTableAsync<CustomScripts>();


            AllModules = await GetAllModules();
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
            if (AllModules.Count != newModules.Count)
            {
                foreach (var module in newModules)
                {
                    if (!AllModules.Any(m => m.ModuleName == module.ModuleName)) // Sprawdź, czy dany moduł już istnieje
                    {
                        connection.InsertAsync(module);
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
            //connection.InsertAsync(AdministratorPage);

            //Modules ResultPage = new Modules
            //{
            //    ModulID = 1,
            //    ModuleName = "TestyWydajnosciowe",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.InsertAsync(ResultPage);

            //Modules CopyBasePage = new Modules
            //{
            //    ModulID = 2,
            //    ModuleName = "KopiowanieBazy",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.InsertAsync(CopyBasePage);
            //Modules CustomScriptsPage = new Modules
            //{
            //    ModulID = 3,
            //    ModuleName = "DodatkoweSkrypty",
            //    ModuleAccess = false,
            //    LastModified = DateTime.Now,
            //    ImgVisualState = "fav.png"
            //};
            //connection.InsertAsync(CopyBasePage);
        }
        public async void AddOrUpdateModule(Modules module)
        {
            int result = 0;
            var existingModule = await connection.FindAsync<Modules>(module.ModulID);
            try
            {

                //if (script.BaseScriptId != 0)
                //if (connection.Query<CopyBaseScripts>($"SELECT * FROM CopyBaseScripts WHERE BaseScriptId={script.BaseScriptId}").Any())
                if (existingModule != null)
                {
                    module.LastModified = DateTime.Now;
                    result = await connection.UpdateAsync(module);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    module.LastModified = DateTime.Now;
                    result = await connection.InsertAsync(module);
                    StatusMessage = $"{result} wiersz dodany!";
                }


            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            Console.WriteLine(StatusMessage);
        }

        public async Task<List<Modules>> GetAllModules()
        {
            try
            {
                try
                {
                    var result3 = await connection.QueryAsync<Modules>("SELECT * FROM Modules");
                    var result2 = connection.ExecuteScalarAsync<int>("SELECT 1");

                    Console.WriteLine("Wynik: " + result2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd: " + ex.Message);
                }
                //return connection.Table<AutoItScript>().ToList();
                var result = await connection.QueryAsync<Modules>("SELECT * FROM Modules");
                if (result == null || result.Count == 0)
                {
                    Console.WriteLine("Brak wyników w tabeli Modules.");
                }
                else
                {
                    Console.WriteLine($"Znaleziono {result.Count} wyników.");
                }
                return result.ToList();
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
            connection.UpdateAsync(script);

        }

        public void DeleteAllModules()
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                connection.ExecuteAsync($"DELETE FROM Modules ");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            AddModules();
        }

        #endregion

        #region CustomScript
        public async Task AddOrUpdateCustomScript(CustomScripts script)
        {
            int result = 0;
            var existingScript = await connection.FindAsync<CustomScripts>(script.CustomScriptId);
            try
            {


                //if (script.BaseScriptId != 0)
                //if (connection.Query<CopyBaseScripts>($"SELECT * FROM CopyBaseScripts WHERE BaseScriptId={script.BaseScriptId}").Any())
                if (existingScript != null)
                {
                    script.CreateScriptDate = DateTime.Now;
                    result = await connection.UpdateAsync(script);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    script.CreateScriptDate = DateTime.Now;
                    result = await connection.InsertAsync(script);
                    StatusMessage = $"{result} wiersz dodany!";
                }


            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            Console.WriteLine(StatusMessage);
        }

        public async Task<List<CustomScripts>> GetAllCustomScripts()
        {
            try
            {
                var result = await connection.QueryAsync<CustomScripts>("SELECT * FROM CustomScripts");
                return result.ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task DeleteCustomScript(int id)
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                await connection.ExecuteAsync($"DELETE FROM CustomScripts where CustomScriptId={id}");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        #endregion


        #region BaseScript
        public async Task AddOrUpdateBaseScript(CopyBaseScripts script)
        {
            int result = 0;
            var existingScript = await connection.FindAsync<CopyBaseScripts>(script.BaseScriptId);
            try
            {


                //if (script.BaseScriptId != 0)
                //if (connection.Query<CopyBaseScripts>($"SELECT * FROM CopyBaseScripts WHERE BaseScriptId={script.BaseScriptId}").Any())
                if (existingScript != null)
                {
                    script.CreateScriptDate = DateTime.Now;
                    result = await connection.UpdateAsync(script);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    script.CreateScriptDate = DateTime.Now;
                    result = await connection.InsertAsync(script);
                    StatusMessage = $"{result} wiersz dodany!";
                }


            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            Console.WriteLine(StatusMessage);
        }

        public async Task<List<CopyBaseScripts>> GetAllBaseScripts()
        {
            try
            {
                //return connection.Table<AutoItScript>().ToList();
                var result = await connection.QueryAsync<CopyBaseScripts>("SELECT * FROM CopyBaseScripts");
                return result;
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task DeleteBaseScript(int id)
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                await connection.ExecuteAsync($"DELETE FROM CopyBaseScripts where BaseScriptId={id}");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }


        #endregion



        #region AppSettings

        public async Task AddOrUpdateAppAdministrator(bool admValue)
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

            
            var existingScript = await connection.FindAsync<AppSettings>(AdministratorSet.SettingsId);


            try
            {
                if (existingScript != null)
                {
                    AdministratorSet.SettingsCreatedAt = DateTime.Now;
                    result = await connection.UpdateAsync(AdministratorSet);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    AdministratorSet.SettingsCreatedAt = DateTime.Now;
                    result = await connection.InsertAsync(AdministratorSet);
                    StatusMessage = $"{result} wiersz dodany!";
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task AddOrUpdateAppSettingsPathExe(AppSettings appSettings)
        {
            int result = 0;
            //appSettings.SettingsId = 0;

            try
            {
                if (appSettings.SettingsValue != null)
                {
                    appSettings.SettingsCreatedAt = DateTime.Now;
                    result = await connection.UpdateAsync(appSettings);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    appSettings.SettingsCreatedAt = DateTime.Now;
                    result = await connection.InsertAsync(appSettings);
                    StatusMessage = $"{result} wiersz dodany!";
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<AppSettings> GetSettings(int id)
        {
            try
            {
                //connection.Execute($"DELETE FROM AppSettings where SettingsName<>{name}");
                return await connection.Table<AppSettings>().FirstOrDefaultAsync(x => x.SettingsId == id);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task<List<AppSettings>> GetAllSettings()
        {
            try
            {
                var result = await connection.QueryAsync<AppSettings>("SELECT * FROM AppSettings");
                return result.ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task DeleteSet()
        {
            try
            {
                //var script = Get(id);
                await connection.ExecuteAsync($"DELETE FROM AppSettings ");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public async Task<AppSettings> GetAdministratorStatus()
        {
            int id = 1;
            try
            {

                return await connection.Table<AppSettings>().FirstOrDefaultAsync(x => x.SettingsId == id);
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


        public async Task AddOrUpdate(AutoItScript script)
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
                    result = await connection.UpdateAsync(script);
                    StatusMessage = $"{result} wiersz zaktualizowany!";
                }
                else
                {
                    script.ScriptCreatedAt = DateTime.Now;
                    result = await connection.InsertAsync(script);
                    StatusMessage = $"{result} wiersz dodany!";
                }


            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            Console.WriteLine(StatusMessage);
        }

        public async Task<List<AutoItScript>> GetAll()
        {
            try
            {
                var result = await connection.QueryAsync<AutoItScript>("SELECT * FROM AutoItScripts");
                return result.ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task<List<AutoItScript>> GetAllFav()
        {
            try
            {
                var result = await connection.QueryAsync<AutoItScript>("SELECT * FROM AutoItScripts where IsFavorite=true");
                return result.ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task<AutoItScript> Get(int id)
        {
            try
            {
                return await connection.Table<AutoItScript>().FirstOrDefaultAsync(x => x.ScriptId == id);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }

            Console.WriteLine(StatusMessage);
            return null;
        }

        public async Task Delete(int id)
        {
            try
            {
                //var script = Get(id);
                //connection.Delete(script);
                await connection.ExecuteAsync($"DELETE FROM AutoItScripts where ScriptId={id}");
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public async Task FavScript(AutoItScript script)
        {
             await connection.UpdateAsync(script);

        }

        #endregion
    }
}
