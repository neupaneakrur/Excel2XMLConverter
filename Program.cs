using Converter.Services;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ExcelToXmlConverter>();

// Configure MIME types for static files
builder.Services.Configure<StaticFileOptions>(options =>
{
    var provider = new FileExtensionContentTypeProvider();
    provider.Mappings[".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    provider.Mappings[".xls"] = "application/vnd.ms-excel";
    options.ContentTypeProvider = provider;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
