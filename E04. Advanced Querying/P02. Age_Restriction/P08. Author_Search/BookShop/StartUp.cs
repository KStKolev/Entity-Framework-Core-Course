﻿namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;
    using System.Data.Common;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            /*DbInitializer.ResetDatabase(db);*/
            Console.WriteLine(GetAuthorNamesEndingIn(db, "dy"));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command) 
        {
            if (!Enum.TryParse<AgeRestriction>(command, true, out var result)) 
            {
                Console.WriteLine($"{command} is not AgeRestricted.");
            }

            var books = context.Books.Where(b => b.AgeRestriction == result)
                    .Select(r => new
                    {
                        r.Title
                    })
                    .OrderBy(r => r.Title)
                    .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        public static string GetGoldenBooks(BookShopContext context) 
        {
            Enum.TryParse<EditionType>("Gold", true, out var result);
            var books = context.Books.Where(b => b.Copies < 5000 && b.EditionType == result)
                .Select(r => new
                {
                    r.Title,
                    r.BookId
                })
                .OrderBy(r => r.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        public static string GetBooksByPrice(BookShopContext context) 
        {
            var books = context.Books.Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();
            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:F2}"));
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year) 
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        public static string GetBooksByCategory(BookShopContext context, string input) 
        {
            string[] categoriesName = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var books = context.BooksCategories
                .Where(bc => categoriesName.Contains(bc.Category.Name.ToLower()))
                .Select(b => new
                {
                    b.Book.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date) 
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .Select(b => new 
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input) 
        {
            var authors = context.Authors.Where(a => a.FirstName.EndsWith(input))
                .Select(a => new 
                {
                    FullName = a.FirstName + " " + a.LastName,
                })
                .OrderBy(a => a.FullName);
            return string.Join(Environment.NewLine, authors.Select(a => a.FullName));
        }
    }
}


