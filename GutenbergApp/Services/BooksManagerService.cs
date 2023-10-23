using GutenbergApp.Data;
using GutenbergApp.Events;
using GutenbergApp.Helpers;
using GutenbergApp.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GutenbergApp.Services
{
    public class BooksManagerService
    {
        GSQLiteHelper gsqliteHelper;
        public BooksManagerService()
        {            
            Initialize();
        }

        public async void Initialize()
        {
            gsqliteHelper = await GSQLiteHelper.Instance;
            
            if(gsqliteHelper != null)
            {
                List<BookItemDb> bookItems = await gsqliteHelper.GetBooksAsync().ConfigureAwait(false);

                if(bookItems == null || bookItems.Count <= 0)
                {
                    _ = DownloadAndSaveBooks();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Books already exist in DB Rows count {bookItems.Count}");
                }
            }
        }

        public async Task<Books> DownloadAndSaveBooks(string uri = APIHelper.BOOKS_API)
        {
            Task<Books> books = CommunicationService.Instance.GetBooksAsync(uri);

            await books;

            if (books.Result != null)
            {
                List<BookItemDb> bookItems = ConvertToBooksDb(books.Result.BooksList);

                Task<int> insertedRows = gsqliteHelper.InsertBooks(bookItems);

                await insertedRows;

                if (insertedRows.Result > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Inserted Rows count {insertedRows.Result}");
                }

                return books.Result;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"No response from server");
            }

            return null;
        }

        public List<BookItem> ConvertBooksDb(List<BookItemDb> bookItemDbs)
        {
            return bookItemDbs.Select(x => new BookItem() 
            {
                BookId = x.BookId,
                Title = x.Title,
                AuthorsAsString = x.Authors,
                PageIndex = x.PageIndex,
                Next = x.NextPageUri,
                Formats = new Formats() { imagejpeg = x.BookCoverImageUri },
                BookCover64EncodedString = x.BookCover64EncodedString
            }).ToList();
        }

        public List<BookItemDb> ConvertToBooksDb(List<BookItem> bookItems)
        {
            return bookItems.Select(x => new BookItemDb()
            {
                BookId = x.BookId,
                PageIndex = string.IsNullOrWhiteSpace(x.Next) ? 1 : Convert.ToInt32(x.Next[x.Next.Length - 1]),
                Authors = x.Authors != null ? string.Join(",", x.Authors.Select(y => y.AuthorName)) : string.Empty,
                BookCoverImageUri = x.Formats.imagejpeg,
                Title = x.Title,
                NextPageUri = x.Next,
                BookCover64EncodedString = x.BookCover64EncodedString
            }).ToList();
        }

        public async void DownloadImages(List<BookItem> bookItems)
        {
            try
            {
                List<BookItem> imagestoDownload = bookItems
                    .Where(x => string.IsNullOrWhiteSpace(x.BookCover64EncodedString)
                                && !string.IsNullOrWhiteSpace(x.Formats?.imagejpeg)).ToList();

                if(imagestoDownload.Count <= 0)
                {
                    return;
                }

                var tasks = imagestoDownload.Select(y =>
                
                    Task.Run(async () =>
                    {
                        System.Diagnostics.Debug.WriteLine($"IsBackground: {Thread.CurrentThread.IsBackground} ");
                        Task<string> imageTask = CommunicationService.Instance.GetBookCoverImage(y.Formats.imagejpeg);

                        await imageTask;

                        if (imageTask.Result != null)
                        {
                            _ = gsqliteHelper.UpdateBookCoverImage(y.BookId, imageTask.Result);
                        }
                    }));

                await Task.WhenAll(tasks);

                MessagingCenter.Send<string, List<BookItem>>("DownloadedImages", "DownloadedImages", bookItems);


                //await Task.Run(() =>
                //{
                //    ParallelLoopResult parallelLoopResult =
                //    Parallel.ForEach(imagestoDownload, async y =>
                //    {
                //         System.Diagnostics.Debug.WriteLine($"IsBackground: {Thread.CurrentThread.IsBackground} ");
                //         Task<string> imageTask = CommunicationService.Instance.GetBookCoverImage(y.Formats.imagejpeg);

                //         await imageTask;

                //         if (imageTask.Result != null)
                //         {
                //             System.Diagnostics.Debug.WriteLine($"BookId: {y.BookId} imageTask Result: {imageTask.Result} ");

                //             _ = gsqliteHelper.UpdateBookCoverImage(y.BookId, imageTask.Result);


                //         }
                //    });

                //    if (parallelLoopResult.IsCompleted)
                //    {
                //        MessagingCenter.Send<string, List<BookItem>>("DownloadedImages", "DownloadedImages", bookItems);
                //    }
                //}) ;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
