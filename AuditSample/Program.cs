using Audit.WebApi;
using AuditSample;
using AuditSample.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Audit
builder.Services.AddMvc(mvc =>
{
    mvc.AddAuditFilter(a => a
                .LogAllActions()
                .IncludeResponseHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody());
});
//Azure Blob sample
builder.Services.Configure<BlobSetting>(builder.Configuration.GetSection("Blob"));
builder.Services.AddSingleton<IBlobService, BlobService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
