var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<UsuarioService>();
builder.Services.AddSingleton<ProductoService>();
builder.Services.AddSingleton<CategoriaService>();
builder.Services.AddSingleton<AdministradorService>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Use CORS policy
app.UseRouting(); // Add this line to enable routing
app.UseAuthorization();
app.MapControllers();
app.Run();
