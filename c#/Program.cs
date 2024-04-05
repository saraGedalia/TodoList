using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(option => option.AddPolicy("AllowAll",
builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (ToDoDbContext DbContext) =>
{
    var data = await DbContext.Items.ToListAsync();
    return Results.Ok(data);
});

app.MapPost("/{name}", async (String name, ToDoDbContext DbContext) =>
{
    Item i = new Item();
    i.Name = name;
    i.IsComplete = false;
    DbContext.Items.Add(i);
    await DbContext.SaveChangesAsync();
    return Results.Ok(DbContext.Items);
});

app.MapPut("/{id}/{isComplete}", async (int id, bool isComplete, ToDoDbContext DbContext) =>
{
    var data = await DbContext.Items.FindAsync(id);
    if (data is null) return Results.NotFound();
    data.IsComplete = isComplete;
    await DbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/{id}", async (int id, ToDoDbContext DbContext) =>
{
    if (await DbContext.Items.FindAsync(id) is Item item)
    {
        DbContext.Items.Remove(item);
        await DbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");

app.Run();
