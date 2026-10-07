using University.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<StudentApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7264/");
});
builder.Services.AddHttpClient<DepartmentAPIService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7264/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
