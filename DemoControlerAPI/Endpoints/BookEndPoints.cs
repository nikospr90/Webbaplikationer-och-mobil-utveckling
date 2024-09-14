using DemoModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Runtime.CompilerServices;

namespace DemoControlerAPI.Endpoints
{
    public static class BookEndPoints
    {
        private const string _route = "/books";
        private const string _tag = "Books";
        public static void MapBookEndPoints(this IEndpointRouteBuilder app)
        {
            // Endpoints Configutrations

            app.MapGet(_route, GetAllBooks)
                .WithSummary("Shows all the books")
                .WithDescription("Here are displayed all the books that are registerd in the database and the status codes of the responses")
                .WithTags(_tag)
                .Produces<Book>(StatusCodes.Status200OK);

            app.MapGet($"{ _route}/{{id}} ", GetBookById)
                .WithSummary("Gets book by Id")
                .WithDescription("Specific book search through it's Id")
                .WithTags(_tag)
                .Produces<Book>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            app.MapPost(_route, AddBook)
                .WithOpenApi()
                .WithSummary("Creates a book")
                .WithDescription("Creating and adding a book to the database")
                .WithTags(_tag)
                .Produces<Book>(StatusCodes.Status201Created)
                .Produces<Book>(StatusCodes.Status409Conflict);

            app.MapPut($"{_route}/{{id}}", UpdateBook)
                .WithSummary("Updates a book's information")
                .WithDescription("Changes a book and send the changes to the database")
                .WithTags(_tag)
                .Produces<Book>(StatusCodes.Status200OK)
                .Produces<Book>(StatusCodes.Status404NotFound);

            app.MapDelete($"{_route}/{{id}}", DeleteBook)
                .WithSummary("Deletes a book")
                .WithDescription("Deletes a book based on it's Id")
                .WithTags(_tag)
                .Produces<Book>(StatusCodes.Status200OK)
                .Produces<Book>(StatusCodes.Status404NotFound);
        }

        // Models
        public record CreateBookRequest(string Title, string Author, string Description);
        public record DetailedBookResponse(string Title, string Author, string Review, string Description, string Isbn);


        //Handlers
        private static Created<DetailedBookResponse> AddBook(CreateBookRequest post, DataContext context)
        {
            var book = new Book
            {
                Title = post.Title,
                Author = post.Author,
                Description = post.Description,
                Review = "No review yet",
                ISBN = "No ISBN yet"
            };

            var response = new DetailedBookResponse(
                book.Title, 
                book.Author, 
                book.Description, 
                book.Review, 
                book.ISBN);

            context.Books.Add(book);
            context.SaveChanges();

            return TypedResults.Created($"{_route}", response);
        }

        // Handlers
        //private static async Task<IResult> AddBook(DataContext bookService, Book book, BookValidator validator)
        //{
        //    var booklist = await bookService.Books.ToListAsync();
        //    foreach (var item in booklist)
        //    {
        //        if (item.ISBN == book.ISBN)
        //        {
        //            return Results.BadRequest("Book with the same ISBN already exists!");
        //        }
        //    }
        //    var result = validator.Validate(book);
        //    if (result.IsValid)
        //    {
        //        bookService.Books.Add(book);
        //        await bookService.SaveChangesAsync();
        //        return Results.Ok(await bookService.Books.ToListAsync());
        //    }
        //    return Results.BadRequest(result.Errors);
        //}

        private static async Task<IResult> GetBookById(int id, DataContext bookService)
        {
            var response = await bookService.Books.FindAsync(id);
            if (response is null)
            {
                return Results.NotFound("Book not found!");
            }
            return Results.Ok(response);
        }

        private static async Task<IResult> UpdateBook(int id, Book book, DataContext bookService)
        {
            var existingBook = await bookService.Books.FindAsync(id);

            if (existingBook == null)
            {
                return Results.NotFound("The book you were looking to update is not found!");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Review = book.Review;
            existingBook.Description = book.Description;
            existingBook.ISBN = book.ISBN;

            bookService.Update(existingBook);
            await bookService.SaveChangesAsync();
            return Results.Ok(existingBook);
        }

        private static async Task<IResult> DeleteBook(int id, DataContext bookService)
        {
            var bookToBeRemoved = await bookService.Books.FindAsync(id);
            if (bookToBeRemoved is null)
            {
                return Results.NotFound("Book not found!");
            }
            bookService.Remove(bookToBeRemoved);
            await bookService.SaveChangesAsync();
            return Results.Ok(bookToBeRemoved);
        }

        public static async Task<IResult> GetAllBooks(DataContext bookService)
        {
            var response = await bookService.Books.ToListAsync();
            return Results.Ok(response);
        }

    }
}
