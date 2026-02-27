using Microsoft.Extensions.Options;
using RpgSheet.Api.Extensions;
using RpgSheet.Api.Infra.Supabase;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.Configure<SupabaseSettings>(builder.Configuration.GetSection("Supabase"));

builder.Services.AddSingleton<Supabase.Client>(sp =>
{
    var s = sp.GetRequiredService<IOptions<SupabaseSettings>>().Value;

    var client = new Supabase.Client(
        s.Url,
        s.ServiceRoleKey,
        new SupabaseOptions { AutoConnectRealtime = false }
    );

    client.InitializeAsync().GetAwaiter().GetResult(); // init sync (ok pro startup)
    return client;
});
builder.Services.AddCors(o =>
{
    o.AddPolicy("frontend", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

