using GeodataService.Services.Geocoding;
using GeodataService.Services.Interfaces;
using GeodataService.Services.ReverseGeocoding;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IReverseGeocodingService, DadataReverseGeocodingService>();
builder.Services.AddTransient<IGeocodingService, OpenstreetmapGeocodingService>();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
