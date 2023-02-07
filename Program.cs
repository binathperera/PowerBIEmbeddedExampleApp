using BlazorAppPowerBI.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<UserInfoGlobalClass>();
builder.Services.AddHttpClient("PowerBIParameterApi", c =>
{
    c.BaseAddress = new Uri("https://api.powerbi.com/");
    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
