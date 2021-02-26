using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WookieBooks.Model;

namespace WookieBooks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates a new book entry on a Post call
        /// </summary>
        /// <param name="title">Title of book</param>
        /// <param name="description">Description of book</param>
        /// <param name="author">Author of book</param>
        /// <param name="coverImage">Cover image of book</param>
        /// <param name="price">Price of book</param>
        /// <returns>HttpStatusCode</returns>
        [HttpPost]
        public HttpStatusCode Create(string title, string description, string author, string coverImage, decimal price)
        {
            string csvString = $"{title},{description},{author},{coverImage},{price}";

            using (StreamWriter file = System.IO.File.AppendText("WookieBooksData.csv"))
            {
                file.WriteLine(csvString);
            }

            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all books on a Get call
        /// </summary>
        /// <returns>All books</returns>
        [HttpGet]
        public IEnumerable<Book> Read()
        {
            List<Book> books = GetBooksCsv();

            return books;
        }

        /// <summary>
        /// Update a book entry that already exists on a Put call
        /// </summary>
        /// <param name="title">Title of book</param>
        /// <param name="description">Description of book</param>
        /// <param name="author">Author of book</param>
        /// <param name="coverImage">Cover image of book</param>
        /// <param name="price">Price of book</param>
        /// <returns>HttpStatusCode</returns>
        [HttpPut]
        public HttpStatusCode Update(string title, string description, string author, string coverImage, decimal price)
        {
            List<Book> books = GetBooksCsv();

            //Looks for book to update
            foreach (Book book in books)
            {
                if (book.Title == title)
                {
                    book.Title = title;
                    book.Description = description;
                    book.Author = author;
                    book.CoverImage = coverImage;
                    book.Price = price;
                }
            }

            SaveBooksCsv(books);

            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Deletes an entry of a book on a Delete call
        /// </summary>
        /// <param name="title">Title of book</param>
        /// <returns>HttpStatusCode</returns>
        [HttpDelete]
        public HttpStatusCode Delete(string title)
        {
            List<Book> books = GetBooksCsv();

            //Removes book from list
            books.RemoveAll(i => i.Title == title);

            SaveBooksCsv(books);

            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Retrieves all books from the CSV file
        /// </summary>
        /// <returns>List of books</returns>
        static List<Book> GetBooksCsv()
        {
            List<Book> books = new List<Book>();

            using (StreamReader reader = new StreamReader("WookieBooksData.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Book book = new Book();
                    book.Title = values[0];
                    book.Description = values[1];
                    book.Author = values[2];
                    book.CoverImage = values[3];
                    book.Price = Decimal.Parse(values[4]);

                    books.Add(book);
                }
            }
            return books;
        }

        /// <summary>
        /// Saves a list of books to the CSV file
        /// </summary>
        /// <param name="books"></param>
        static void SaveBooksCsv(List<Book> books)
        {
            string csvString = "";

            foreach (Book book in books)
            {
                csvString = csvString + $"{book.Title},{book.Description},{book.Author},{book.CoverImage},{book.Price}\n";
            }

            System.IO.File.WriteAllText("WookieBooksData.csv", csvString);
        }
    }
}
