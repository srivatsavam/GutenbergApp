using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GutenbergApp.Models
{
    public class Books
    {
        public int Count { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }

        public List<BookItem> BooksList { get; set; }
    }

    public class BookItem : INotifyPropertyChanged
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public int PageIndex { get; set; }
        public List<Author> Authors { get; set; }

        private string authorsAsString;
        public string AuthorsAsString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(authorsAsString) && Authors != null && Authors.Count > 0)
                {
                    authorsAsString = string.Join(", ", Authors.Select(x => x.AuthorName));
                }
                return authorsAsString;
            }
            set
            {
                authorsAsString = value;
            }
        }

        public List<Author> Translators { get; set; } 

        public List<string> Subjects { get; set; }

        public List<string> BookShelves { get; set; }

        public List<string> Languages { get; set; }

        public bool Copyright { get; set; }

        public string MediaType { get; set; }

        public Formats Formats { get; set; }

        public int DownloadCount { get; set; }

        public string Next { get; set; }

        private string bookCover64EncodedString;
        public string BookCover64EncodedString 
        {
            get => bookCover64EncodedString; 
            set
            {
                bookCover64EncodedString = value;
                OnPropertyChanged(nameof(BookCover64EncodedString));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Author
    {
        public string AuthorName { get; set; }

        public int BirthYear { get; set; }

        public int DeathYear { get; set; }
    }

    public class Formats
    {
        public string applicationxmobipocketebook { get; set; }

        public string applicationepubzip { get; set; }

        public string texthtml { get; set; }

        public string applicationoctetstream { get; set; }

        public string imagejpeg { get; set; }

        public string textplain { get; set; }

        public string textplaincharsetusascii { get; set; }

        public string applicationrdfxml { get; set; }

        public string texthtmlcharsetutf8 { get; set; }

        public string textplaincharsetutf8 { get; set; }

        public string texthtmlcharsetiso88591 { get; set; }

        public string textplaincharsetiso88591 { get; set; }
    }
}
