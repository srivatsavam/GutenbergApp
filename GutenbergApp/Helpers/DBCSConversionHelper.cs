using GutenbergApp.Data;
using GutenbergApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GutenbergApp.Helpers
{
    public class DBCSConversionHelper
    {
        public List<Books> GetBooks(List<BookItemDb> bookDbItems)
        {
            return bookDbItems.Select(x => new Books() 
            {
                
            }).ToList();
        }
    }
}
