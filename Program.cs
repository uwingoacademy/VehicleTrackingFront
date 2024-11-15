using Frontend.Helper;
using Frontend.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<LocalizationMiddleware>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.InitializeClientBaseAddress(builder.Configuration);
builder.Services
    .AddRazorPages()
    .AddViewLocalization();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr-TR"),
        new CultureInfo("en-US"),
        new CultureInfo("kr-KR")
    };

    //options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0]);
    options.DefaultRequestCulture = new RequestCulture(new CultureInfo("tr-TR"));
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var localizationOptions = services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
    app.UseRequestLocalization(localizationOptions);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Registers HttpClient in the DI container
app.UseMiddleware<LocalizationMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Urls.Add("http://0.0.0.0:5010");
app.Run();
