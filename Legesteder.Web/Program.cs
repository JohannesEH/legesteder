using Legesteder.Web.Services;
using Tailwind;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IDawaService, DawaService>();
builder.Services.AddScoped<IPlaygroundService, PlaygroundService>();
builder.Services.AddHttpClient<IDawaService, DawaService>(client =>
{
    client.BaseAddress = new Uri("https://api.dataforsyningen.dk/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    app.RunTailwind("tailwind");
}

app.Run();