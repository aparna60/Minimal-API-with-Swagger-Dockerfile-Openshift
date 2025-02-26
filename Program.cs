using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Api", Version = "v1" });
}
);

var app = builder.Build();

// Enable Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book API V1");
    });
}




List<Book> books = new List<Book>();
books = new List<Book>()
{
    new Book
    {
        Id = 1,
        Title="The Kite Runner",
        Author="ABC"
    },
    new Book
    {
        Id = 2,
        Title="Microsoft",
        Author="XYZ"
    }
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

