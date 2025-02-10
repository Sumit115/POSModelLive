
using LMS.Data;
using LMS.IRepository;
using LMS.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.IRepository.Option;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Repository;
using SSRepository.Repository.Master;
using SSRepository.Repository.Option;
using SSRepository.Repository.Report;
using SSRepository.Repository.Transaction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    // Add global authorization filter
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddMvc(options =>
{

    options.MaxModelBindingCollectionSize = int.MaxValue;

});
builder.Services.Configure<FormOptions>(x => x.ValueCountLimit = 22048);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Auth";  // Path to the login page
            options.LogoutPath = "/Auth/Logout"; // Path to the logout page
            options.Cookie.HttpOnly = true;        // Makes cookie inaccessible from JavaScript for security
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use secure cookies (HTTPS only)
            options.ExpireTimeSpan = TimeSpan.FromMinutes(720); // Set expiration time for the cookie
            options.SlidingExpiration = true;      // Extends cookie expiration on active requests
        });

builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Formatting = Formatting.Indented;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

    var resolver = options.SerializerSettings.ContractResolver;
    if (resolver != null)
    {
        var res = resolver as DefaultContractResolver;
        res.NamingStrategy = null;  // <<!-- this removes the camelcasing
    }
});

builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<ITableMasRepository, TableMasRepository>();>();
//builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddDbContext<ssodbContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(80);//You can set Time  
});
//builder.Services.AddScoped<MenuService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IGridLayoutRepository, GridLayoutRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryGroupRepository, CategoryGroupRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
builder.Services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
builder.Services.AddScoped<ISalesChallanRepository, SalesChallanRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IProductLotRepository, ProductLotRepository>();
builder.Services.AddScoped<ISalesStockRepository, SalesStockRepository>();
builder.Services.AddScoped<IPurchaseStockRepository, PurchaseStockRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ISalesCrNoteRepository, SalesCrNoteRepository>();
//builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IAccountGroupRepository, AccountGroupRepository>();
builder.Services.AddScoped<IAccountMasRepository, AccountMasRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IStockDetailReportRepository, StockDetailReportRepository>();
builder.Services.AddScoped<IRateEndStockRepository, RateEndStockRepository>();
builder.Services.AddScoped<ISalesTransactionRepository, SalesTransactionRepository>();
builder.Services.AddScoped<IPurchaseTransactionRepository, PurchaseTransactionRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<ISalesOrderStockRepository, SalesOrderStockRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IImportRepository, ImportRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<ILocationTransferRequestRepository, LocationTransferRequestRepository>();
builder.Services.AddScoped<ILocationTransferInvoiceRepository, LocationTransferInvoiceRepository>();
builder.Services.AddScoped<ILocationRequestRepository, LocationRequestRepository>();
builder.Services.AddScoped<ILocationReceiveRepository, LocationReceiveRepository>();
builder.Services.AddScoped<IJobWorkRepository, JobWorkRepository>();
builder.Services.AddScoped<IJobWorkIssueRepository, JobWorkIssueRepository>();
builder.Services.AddScoped<IJobWorkReceiveRepository, JobWorkReceiveRepository>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseBrowserLink();
    //app.UseDeveloperExceptionPage();

    app.UseExceptionHandler("/Auth/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //app.UseExceptionHandler("/Auth/Error");
}
app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        ctx.Request.Path = "/Auth/index";
    }
});

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{id2?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});

app.Run();
