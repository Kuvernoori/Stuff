using Microsoft.EntityFrameworkCore;
using WEB;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/api/stuffs", async (ApplicationContext db) =>
{
    var stuffs = await db.Stuffs.ToListAsync();
    return Results.Ok(stuffs);
});

app.MapGet("/api/stuffs/{id:int}", async (int id, ApplicationContext db) =>
{
    var stuff = await db.Stuffs.FirstOrDefaultAsync(u => u.Id == id);
    if (stuff == null) return Results.NotFound(new { message = "Stuff not found" });
    return Results.Ok(stuff);
});

app.MapDelete("/api/stuffs/{id:int}", async (int id, ApplicationContext db) =>
{
    var stuff = await db.Stuffs.FirstOrDefaultAsync(u => u.Id == id);
    if (stuff == null) return Results.NotFound(new { message = "Stuff not found" });
    db.Stuffs.Remove(stuff);
    await db.SaveChangesAsync();
    return Results.Ok(new { message = "Stuff deleted", stuff });
});

app.MapPost("/api/stuffs", async (Stuff stuff, ApplicationContext db) =>
{
    await db.Stuffs.AddAsync(stuff);
    await db.SaveChangesAsync();
    return Results.Ok(stuff);
});
app.MapPut("/api/stuffs", async (Stuff stuffData, ApplicationContext db) =>
{
    var stuff = await db.Stuffs.FirstOrDefaultAsync(u => u.Id == stuffData.Id);
    if (stuff == null) return Results.NotFound(new { message = "Stuff not found" });
    stuff.Name = stuffData.Name;
    stuff.Category = stuffData.Category;
    await db.SaveChangesAsync();
    return Results.Ok(stuff);
});

app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
