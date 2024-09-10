using DemoModels;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // this is where magic happens
        var books = Book.GetSampleData();
        app.MapGet("/books", () => Results.Ok(books));
        app.MapPost("/books", (Book book) =>
        {
            book.Id = books.Max(b => b.Id) + 1;
            books.Add(book);
            return Results.Created($"/books/{book.Id}", book);
        });

        app.MapGet("/books{id}", (int id) =>
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(book);
        });

        app.Run();
    }
}