using Microsoft.FluentUI.AspNetCore.Components;
using MediaServer.Web.Components;
using Microsoft.Extensions.Options;
using MediaServer.Web.Services;
using MediaServer.Web.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient(Options.DefaultName, 
        client =>
        {
            var baseAddress = builder.Configuration["Api:BaseAddress:Internal"];
            if (string.IsNullOrWhiteSpace(baseAddress))
                throw new NullReferenceException(nameof(baseAddress));
            client.BaseAddress = new Uri(baseAddress);
        }
    );
// Add services to the container.
builder.Services.AddScoped<PlayerState>();
builder.Services.Add(ServiceDescriptor.Scoped(typeof(IStateService<>), typeof(StateService<>)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
