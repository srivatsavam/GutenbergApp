using System;
using System.Collections.Generic;
using System.Text;

namespace GutenbergApp.Events
{
    public class BookCoverDownloadedEvent
    {
        public int BookId { get; set; }

        public string ImageAsString { get; set; }
    }
}
