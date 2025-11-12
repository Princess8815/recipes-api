using Recipes_Api.Repositories;
using Recipes_Api.Data;
using Recipes_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add RecipeService
builder.Services.AddScoped<RecipeService>();

// ⚙️ CORS: allow MAUI app to access API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMauiClient", policy =>
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true) // dev-friendly, you can restrict later
            .AllowCredentials());
});

// Toggle data source
var dataSource = builder.Configuration.GetValue<string>("DataSource");
if (dataSource == "Sql")
{
    builder.Services.AddScoped<IRecipeRepository, SqlRecipeRepository>();
    DatabaseInitializer.Initialize(builder.Configuration);
}
else
{
    builder.Services.AddScoped<IRecipeRepository, JsonRecipeRepository>();
}

var app = builder.Build();

// 🔥 Enable CORS before mapping controllers
app.UseCors("AllowMauiClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();




