using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Repository;
using SSRepository.Repository.Master;
using SSRepository.Repository.Report;
using SSRepository.Repository.Transaction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
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
builder.Services.AddSession();

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
builder.Services.AddScoped<ILoginRepository, LoginRepository>(); 
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
//builder.Services.AddScoped<ITranBaseRepository, TranBaseRepository>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDistributedMemoryCache();

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
app.UseSession();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});

app.Run();
