namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Collections;
    using System.Data.Common;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            /*DbInitializer.ResetDatabase(db);*/
            Console.WriteLine(GetGoldenBooks(db));
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
    }
}


