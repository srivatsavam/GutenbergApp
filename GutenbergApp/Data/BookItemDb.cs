using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GutenbergApp.Data
{
    [Table("Books")]
    [Preserve(AllMembers = true)]
    public class BookItemDb
    {
        [PrimaryKey]
        public int BookId { get; set; }

        public int PageIndex { get; set; }
        
        public string Title { get; set; }

        public string Authors { get; set; }

        public string BookCoverImageUri { get; set; }

        public string NextPageUri { get; set; }

        public string BookCover64EncodedString { get; set; }
    }
}
