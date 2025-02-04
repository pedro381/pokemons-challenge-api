using Microsoft.EntityFrameworkCore;
using Refit;
using PokemonsChallenge.Repository;
using PokemonsChallenge.Service;
using PokemonsChallenge.Service.Interfaces;
using PokemonsChallenge.Repository.Interfaces;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Adiciona suporte ao CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=pokemon.db"));

builder.Services.AddRefitClient<IPokeApiService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["pokeapi"]!));

builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();

var app = builder.Build();

// Habilita o Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Sempre mostra os erros internos detalhados
app.UseDeveloperExceptionPage();  // Isso garante que erros detalhados sejam visíveis na API

// Habilita o serviço para servir arquivos estáticos a partir da pasta "teste"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "teste")),
    RequestPath = "/teste"
});

// Aplica a política de CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
