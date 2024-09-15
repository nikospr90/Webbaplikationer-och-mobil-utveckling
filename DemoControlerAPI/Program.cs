using DemoControlerAPI;
using DemoControlerAPI.Endpoints;
using DemoControlerAPI.Migrations;
using DemoModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new() 
            { 
                Title = "Database Documentation", 
                Version = "v1", 
                Description= "Here you can find documentaion of the database",
                Contact = new()
                {
                    Name = "Nikos",
                    Email = "nick_prevenios@hotmail.com",
                }   
            });
        });
        builder.Services.AddSingleton<IBookService, BookService>();
        builder.Services.AddDbContext<DataContext>();
        builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("allowAll", policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelExpandDepth(0);
            });
            //app.UseCors("allowAll");
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapGet("/", () => Results.File("./html-test-page.html, text/html"));
        app.MapBookEndPoints();

        app.Run();
    }
}
