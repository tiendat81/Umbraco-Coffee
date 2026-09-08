using Umbraco.Cms.Web.Common.ApplicationBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

// ============================================
// ADD THIS: Multilingual support
// ============================================
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "en-US", "vi-VN" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// Umbraco will handle culture based on URL (e.g., /en/ or /vi/)
// but we still need to register the Localization middleware
// ============================================

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

// ============================================
// ADD THIS: Localization middleware
// ============================================
app.UseRequestLocalization(localizationOptions);
// ============================================

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
