using GutenbergApp.Data;
using GutenbergApp.Events;
using GutenbergApp.Helpers;
using GutenbergApp.MockData;
using GutenbergApp.Models;
using GutenbergApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GutenbergApp.ViewModels
{
    public class CatalogPageViewModel : BaseViewModel
    {
        #region Properties

        private ObservableCollection<BookItem> _books;
        public ObservableCollection<BookItem> Books
        {
            get => _books;

            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }
        
        public ICommand BooksCollectionThresholdReached
        {
            get;
            set;
        }

        GSQLiteHelper gssqliteHelper;
        BooksManagerService booksManager;

        #endregion

        public CatalogPageViewModel()
        {
            booksManager = DependencyService.Resolve<BooksManagerService>();
            
            BooksCollectionThresholdReached = new Command(this.DownloadMoreItems);

            MessagingCenter.Subscribe<string, List<BookItem>>("DownloadedImages", "DownloadedImages", (booksManager, x) => UpdateBookCovers(x));

            Initialize();
        }

        public async void Initialize()
        {
            gssqliteHelper = await GSQLiteHelper.Instance;

            Task<List<BookItemDb>> bookItemsTask = gssqliteHelper.GetBooksAsync();

            await bookItemsTask;

            if(booksManager == null)
            {
                booksManager = DependencyService.Resolve<BooksManagerService>();
            }

            if(booksManager != null && bookItemsTask.Result != null && bookItemsTask.Result.Count > 0)    
            {
                List<BookItem> books = booksManager.ConvertBooksDb(bookItemsTask.Result);

                if(books != null && books.Count > 0)
                {
                    Books = new ObservableCollection<BookItem>(books);

                    Parallel.ForEach(Books.ToList(), x => System.Diagnostics.Debug.WriteLine($"BookId: {x.BookId} IsBase64Null: {string.IsNullOrWhiteSpace(x.BookCover64EncodedString)} "));
                }

                DownloadBookCovers();
            }
        }


        private async void DownloadMoreItems()
        {
            BookItem bookItem = Books[Books.Count - 1];

            string uri = APIHelper.GetPageUri(bookItem.PageIndex + 1);

            Task<Books> booksTask = booksManager.DownloadAndSaveBooks(uri);

            Books books = await booksTask;

            if(books != null && books.Count > 0)
            {
                foreach(var item in booksTask.Result.BooksList)
                {
                    Books.Add(item);
                }
            }

            DownloadBookCovers();
        }


        private void DownloadBookCovers()
        {
            try
            {
                //Task<List<BookCoverDownloadedEvent>> bookCoverTask = booksManager.DownloadImages(new List<BookItem>() { Books[0] });

                booksManager.DownloadImages(Books.ToList());

                //await bookCoverTask.ConfigureAwait(false);

                //if (bookCoverTask.Result != null && bookCoverTask.Result.Count > 0)
                //{
                //    BookItem book;
                //    int index;

                //    foreach (var item in bookCoverTask.Result)
                //    {
                //        book = Books.FirstOrDefault(x => x.BookId == item.BookId);

                //        index = Books.IndexOf(book);

                //        Books[index].BookCover64EncodedString = item.ImageAsString;
                //    }
                //}
            }
            catch(Exception ex)
            {

            }
        }

        private void UpdateBookCovers(List<BookItem> bookItems)
        {
            BookItem book;
            int index;

            foreach(BookItem item in bookItems)
            {
                Task<BookItemDb> bookItemTask = gssqliteHelper.GetBookAsync(item.BookId);

                bookItemTask.Wait();

                book = Books.FirstOrDefault(x => x.BookId == item.BookId);

                index = Books.IndexOf(book);

                Books[index].BookCover64EncodedString = bookItemTask.Result.BookCover64EncodedString;
            }
        }
    }
}
