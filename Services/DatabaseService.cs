using SQLite;
using ExpenseTracker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<ExpenseItem>().Wait();
        }

        public Task<List<ExpenseItem>> GetExpenseItemsAsync(int userId)
        {
            return _database.Table<ExpenseItem>().Where(item => item.UserId == userId).ToListAsync();
        }

        public Task<int> SaveExpenseItemAsync(ExpenseItem item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteExpenseItemAsync(ExpenseItem item)
        {
            return _database.DeleteAsync(item);
        }

        public Task<int> DeleteAllExpenseItemsAsync(int userId)
        {
            return _database.Table<ExpenseItem>().Where(item => item.UserId == userId).DeleteAsync();
        }

        public Task<User> GetUserAsync(string username)
        {
            return _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public Task<User> GetUserName(int userId)
        {
            return _database.Table<User>().Where(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public async Task<int> UpdateUserAsync(int userId, string newUsername, string newPassword)
        {
            var user = await _database.Table<User>().Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user != null)
            {
                user.Username = newUsername;
                user.Password = newPassword;
                return await _database.UpdateAsync(user);
            }

            return 0;
        }

        public Task<int> DeleteUserAsync(User user)
        {
            return _database.DeleteAsync(user);
        }
    }
}
