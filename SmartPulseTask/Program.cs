using SmartPulseTask.Services;
using SmartPulseTask.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ITransactionApiService, TransactionApiService>();
builder.Services.AddScoped<ITransactionCalculationService, TransactionCalculationService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Index}/{id?}");

app.Run();
