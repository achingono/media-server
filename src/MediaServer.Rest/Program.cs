using MediaServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MediaServer;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Library");
var maxTop = builder.Configuration.GetValue<int>("Api:MaxTop", 200);

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    // suppress child validation for `Badge` members
    opt.ModelMetadataDetailsProviders.Add(
        new SuppressChildValidationMetadataProvider(typeof(Badge)));
})
.AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(maxTop))
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EntityContext>(options => options.UseSqlite(connectionString));

// Reorder the input formatters to place the JSON Patch formatter at the beginning
builder.Services.PostConfigure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
{
    var formatter = options.InputFormatters
                            .OfType<NewtonsoftJsonPatchInputFormatter>()
                            .First();
    // place the formatter at the beginning of the collection
    options.InputFormatters.Remove(formatter);
    options.InputFormatters.Insert(0, formatter);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    //app.UseSwagger();
    //app.UseSwaggerUI();
    using (var scope = app.Services.CreateScope())
    {
        var context = scope!.ServiceProvider.GetRequiredService<EntityContext>();
        context!.Database.EnsureCreated();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
