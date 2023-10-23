using GutenbergApp.Json;
using GutenbergApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GutenbergApp.Helpers
{
    public static class JsonCSConversion
    {
        public static Books PraseJson(BooksJson booksJson)
        {
            return new Books()
            {
                Count = booksJson.count,
                Next = booksJson.next,
                Previous = booksJson.previous,
                BooksList = booksJson.results.Select(x => new BookItem()
                {
                    BookId = x.id,
                    Title = x.title,

                    Authors = x.authors.Select(y => new Author()
                    {
                        AuthorName = y.name,
                        BirthYear = y.birth_year != null ? y.birth_year.Value : 0,
                        DeathYear = y.death_year != null ? y.death_year.Value : 0,
                    }).ToList(),

                    Translators = x.translators.Select(y => new Author()
                    {
                        AuthorName = y.name,
                        BirthYear = y.birth_year != null ? y.birth_year.Value : 0,
                        DeathYear = y.death_year != null ? y.death_year.Value : 0,
                    }).ToList(),

                    Subjects = new List<string>(x.subjects),

                    BookShelves = new List<string>(x.bookshelves),

                    Languages = new List<string>(x.languages),

                    Copyright = x.copyright,

                    MediaType = x.media_type,

                    Formats = new Formats()
                    {
                        applicationepubzip = x.formats.applicationepubzip,

                        applicationxmobipocketebook = x.formats.applicationxmobipocketebook,

                        texthtml = x.formats.texthtml,

                        applicationoctetstream = x.formats.applicationoctetstream,

                        imagejpeg = x.formats.imagejpeg,

                        textplain = x.formats.textplain,

                        textplaincharsetusascii = x.formats.textplaincharsetusascii,

                        applicationrdfxml = x.formats.applicationrdfxml,

                        texthtmlcharsetutf8 = x.formats.textplaincharsetutf8,

                        textplaincharsetutf8 = x.formats.texthtmlcharsetutf8,

                        texthtmlcharsetiso88591 = x.formats.texthtmlcharsetiso88591,

                        textplaincharsetiso88591 = x.formats.textplaincharsetiso88591
                    },

                    DownloadCount = x.download_count
                }).ToList()
            };
        }

        public static BooksJson GetJson(Books books)
        {
            return null;
        }
    }
}
