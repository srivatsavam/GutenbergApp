using System;
using System.Collections.Generic;
using System.Text;

namespace GutenbergApp.Helpers
{
    public static class APIHelper
    {
        public const string BOOKS_API = "https://gutendex.com/books";

        public static string GetPageUri(int index)
        {
            return "https://gutendex.com/books/?page=" + index;
        }
    }
}
