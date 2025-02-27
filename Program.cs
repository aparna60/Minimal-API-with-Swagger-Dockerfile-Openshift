using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Api", Version = "v1" });
}
);

var app = builder.Build();

// Enable Swagger UI in development mode
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book API V1");
        c.RoutePrefix = "swagger";
    });

//}

List<Book> books = new List<Book>();
books = new List<Book>() 
{
   
            new Book { Id = 1, Title = "The Kite Runner", Author = "Khaled Hosseini" },
            new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee" },
            new Book { Id = 3, Title = "1984", Author = "George Orwell" },
            new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen" },
            new Book { Id = 5, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" },
            new Book { Id = 6, Title = "Moby-Dick", Author = "Herman Melville" },
            new Book { Id = 7, Title = "War and Peace", Author = "Leo Tolstoy" },
            new Book { Id = 8, Title = "The Catcher in the Rye", Author = "J.D. Salinger" },
            new Book { Id = 9, Title = "The Lord of the Rings", Author = "J.R.R. Tolkien" },
            new Book { Id = 10, Title = "The Alchemist", Author = "Paulo Coelho" }
        
};

// Get all books
app.MapGet("/books", ()=>  Results.Ok(books));

// Get a book by ID
app.MapGet("/books/{id}", (int id) =>
{
   var book= books.Find(x=> x.Id == id);
   return book is not null ?  Results.Ok(book) : Results.NoContent();
});

// Add a new book
app.MapPost("/books", ([FromBody] Book book) =>
{
    book.Id= books.Count + 1;
    books.Add(book);
    return Results.Created($"/books/{book.Id}", book);
});


// Update a book
app.MapPut("/books/{id}", ([FromBody] Book b, int id) =>
{
    var book = books.Find(b => b.Id.Equals(id));
    if (book is null) return Results.NotFound();
    
        book.Title = b.Title;
        book.Author = b.Author;
    
    return Results.Ok(book);
});

// Delete a book
app.MapDelete("/Books/{id}", (int id)=>
{
    var book = books.FirstOrDefault(x => x.Id == id);
    if (book is null) return Results.NotFound();
    books.Remove(book);
    return Results.NoContent();
});

app.Run();

record Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

