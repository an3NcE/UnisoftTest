﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnisoftTest
{
    public static class Constants
    {
        private const string DBFileName = "DBUnisoftTest.db3";
        const string passKey = "HasłoDoBazySQLite";

       // public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        
        public static string DatabasePath
        {
            get
            {
                string appDirectory = AppContext.BaseDirectory;
                string dataFolder = Path.Combine(appDirectory, "Data");

                if (!Directory.Exists(dataFolder))
                {
                    Directory.CreateDirectory(dataFolder); 
                }

                return Path.Combine(dataFolder, DBFileName);
            }
        }
        public static async Task<string> GetDatabasePasswordAsync()
        {
            //var password = await SecureStorage.GetAsync(passKey);
            //if (password == null)
            //{
            //    string passwordToSave = "TwojeSuperSilneHasłoDoBazy";
            //    await SecureStorage.SetAsync(passKey, passwordToSave);
            //    password = await SecureStorage.GetAsync(passKey);
            //}
            //
            var password= "TwojeSuperSilneHasłoDoBazy";
            return password;
        }

    }
}
