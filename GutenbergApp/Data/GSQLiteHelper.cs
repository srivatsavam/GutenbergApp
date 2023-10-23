using GutenbergApp.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GutenbergApp.Data
{
    public class GSQLiteHelper
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<GSQLiteHelper> Instance = new AsyncLazy<GSQLiteHelper>(async () =>
        {
            var instance = new GSQLiteHelper();
            CreateTableResult result = await Database.CreateTableAsync<BookItemDb>();
            return instance;
        });

        public GSQLiteHelper()
        {
            Database = new SQLiteAsyncConnection(SQLiteConstants.DatabasePath, SQLiteConstants.Flags);
        }

        public Task<List<BookItemDb>> GetBooksAsync()
        {
            return Database.Table<BookItemDb>().ToListAsync();
        }

        public Task<BookItemDb> GetBookAsync(int id)
        {
            return Database.Table<BookItemDb>().Where(x => x.BookId == id).FirstOrDefaultAsync();
        }

        public Task<int> InsertBook(BookItemDb bookItemDb)
        {
            Task<BookItemDb> task = GetBookAsync(bookItemDb.BookId);

            task.Wait();

            if(task.Result == null)
            {
                return Database.InsertAsync(bookItemDb);
            }
            else
            {
                return Database.UpdateAsync(bookItemDb);
            }
        }

        public Task<int> InsertBooks(List<BookItemDb> books)
        {
            return Database.InsertAllAsync(books, true);
        }

        public Task<int> UpdateBooks(List<BookItemDb> books)
        {
            return Database.UpdateAllAsync(books, true);
        }

        public Task<int> UpdateBookCoverImage(int bookId, string imageString)
        {
            Task<int> task = Database.ExecuteAsync($"update Books set BookCover64EncodedString = '{imageString}' where BookId = '{bookId}' ");

            task.Wait();

            return task;
        }
    } 
}
