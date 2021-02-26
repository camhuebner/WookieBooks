using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WookieBooks.Model
{
    /// <summary>
    /// Used to store book data
    /// </summary>
    public class Book
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public decimal Price { get; set; }

    }
}
