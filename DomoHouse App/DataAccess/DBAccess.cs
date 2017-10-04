
using System;
using System.Collections.Generic;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Async;
using System.Threading.Tasks;
using SQLite.Net.Platform.XamarinAndroid;

namespace Mica.Droid.DataAccess
{
    public class DBAccess
    {
        private SQLiteAsyncConnection dbConn;
        public string StatusMessage { get; set; }
        

        public DBAccess()
        {
            try
            {
                string dbPath = FileAccessHelper.GetLocalFilePath("domohouse.db3");
                ISQLitePlatform sqlitePlatform = new SQLitePlatformAndroid();
                if (dbConn == null)
                {
                    var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
                        new SQLiteConnectionWithLock
                        (
                            sqlitePlatform,
                            new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
                        ));

                    dbConn = new SQLiteAsyncConnection(connectionFunc);
                    dbConn.CreateTableAsync<Usuario>();
                }
            }
            catch (Exception e)
            {
                StatusMessage = e.Message;
            }
        }

        public async Task AddNewUserAsync(Usuario user)
        {
            try
            {
                var result = await dbConn.InsertAsync(user);
            }
            catch (Exception){ }
        }

        public async Task<Usuario> GetUser(string email)
        {
            List<Usuario> users = await dbConn.Table<Usuario>().ToListAsync();
            foreach (Usuario user in users)
            {
                if (user.Email.Equals(email))
                    return user;
            }
            return null;
        }

        public async Task<Usuario> GetUserOnline()
        {
            List<Usuario> users = await dbConn.Table<Usuario>().ToListAsync();
            foreach (Usuario user in users)
            {
                if (user.IsOnApp)
                    return user;
            }
            return null;
        }

        public async Task<List<Usuario>> GetAllUserAsync()
        {
            List<Usuario> users = await dbConn.Table<Usuario>().ToListAsync();
            return users;
        }

        public async Task UpdateUserAsync(Usuario user)
        {
            try
            {
                var result = await dbConn.UpdateAsync(user);
            }
            catch (Exception) { }
        }

        public async Task DeleteUserAsync(Usuario user)
        {
            try
            {
                var result = await dbConn.DeleteAsync<Usuario>(user);
            }
            catch (Exception) { }
        }

        public async Task DeleteAllAsync()
        {
            try
            {
                await dbConn.DeleteAllAsync<Usuario>();
            }
            catch (Exception) { }
        }
    }
}