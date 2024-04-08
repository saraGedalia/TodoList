using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
//גישה למסד
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
        new MySqlServerVersion(new Version(8, 0, 26))));

builder.Services.AddScoped<Items>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
//swagger -בניית ה
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
//הוספת הרשאת גישה לכולם
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));
var app = builder.Build();//משתנה לפעולות
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("corsapp");
        app.UseAuthorization();
//הצגה
app.MapGet("/items",  (ToDoDbContext db) =>
{
    return  db.Items;
});
//הוספה
app.MapPost("/{name}", async (String name, ToDoDbContext DbContext) =>
{
    Items i = new Items();
    i.Name = name;
    i.IsComplete = false;
    DbContext.Items.Add(i);
    await DbContext.SaveChangesAsync();
    return Results.Ok(DbContext.Items);
});
//עדכון
app.MapPut("/{id}/{isComplete}", async (int id, bool isComplete, ToDoDbContext DbContext) =>
{
    var data = await DbContext.Items.FindAsync(id);
    if (data is null) return Results.NotFound();
    data.IsComplete = isComplete;
    await DbContext.SaveChangesAsync();
    return Results.NoContent();
});
//מחיקה
app.MapDelete("/{id}", async (int id, ToDoDbContext DbContext) =>
{
    if (await DbContext.Items.FindAsync(id) is Items item)
    {
        DbContext.Items.Remove(item);
        await DbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapGet("/", () => "Api is Running");
app.Run();
