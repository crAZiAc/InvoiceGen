using Azure.Data.Tables;
using InvoiceGen.Api.Interfaces;
using InvoiceGen.Api.Services;
using InvoiceGen.App.Areas.Identity;
using InvoiceGen.App.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using System.Globalization;
using Microsoft.Extensions.Azure;
using InvoiceGen.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
//{
//    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
//    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
//});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddSingleton<IInvoiceService>(InitializeInvoiceTableClientInstanceAsync(builder.Configuration).GetAwaiter().GetResult());
builder.Services.AddSingleton<IAddressService>(InitializeAddressTableClientInstanceAsync(builder.Configuration).GetAwaiter().GetResult());
builder.Services.AddSingleton<ISettingsService>(InitializeSettingsTableClientInstanceAsync(builder.Configuration).GetAwaiter().GetResult());
builder.Services.AddSingleton<SelectedSellerId>(s => new SelectedSellerId());
builder.Services.AddBlazorBootstrap();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var cultureInfo = new CultureInfo("nl-NL");
cultureInfo.NumberFormat.CurrencySymbol = "�";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.Run();


static async Task<InvoiceService> InitializeInvoiceTableClientInstanceAsync(ConfigurationManager configManager)
{
    string connectionString = configManager["StorageAccount:table"]!;
    var serviceClient = new TableServiceClient(connectionString);

    InvoiceService tableDbService = new InvoiceService(serviceClient, connectionString);
    return tableDbService;
}

static async Task<AddressService> InitializeAddressTableClientInstanceAsync(ConfigurationManager configManager)
{
    string connectionString = configManager["StorageAccount:table"]!;
    var serviceClient = new TableServiceClient(connectionString);

    AddressService tableDbService = new AddressService(serviceClient, connectionString);
    return tableDbService;
}

static async Task<InvoiceSettingsService> InitializeSettingsTableClientInstanceAsync(ConfigurationManager configManager)
{
    string connectionString = configManager["StorageAccount:table"]!;
    var serviceClient = new TableServiceClient(connectionString);

    InvoiceSettingsService tableDbService = new InvoiceSettingsService(serviceClient, connectionString);
    return tableDbService;
}