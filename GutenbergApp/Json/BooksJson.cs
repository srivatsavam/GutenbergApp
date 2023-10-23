using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GutenbergApp.Json
{
    public class BooksJson
    {
        public int count { get; set; }

        public string next { get; set; }

        public string previous { get; set; }

        public List<BookItemJson> results { get; set; }
    }
}
