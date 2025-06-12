using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Application.Services;
using MoviesChallenge.Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") //A que front le das permiso
              .AllowAnyHeader() //Permite headers personalizados
              .AllowAnyMethod(); //Permite todos los metodos, get post etc
    });
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IMovieService, MoviesService>();
builder.Services.AddScoped<IMovieApiClient, MovieApiClient>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
