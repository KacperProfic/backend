using ApplicationCore.Commons.Repository;
using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;
using Infrastructure.Memory;
using Infrastructure.Memory.Generators;
using Infrastructure.Memory.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IntGenerator>();
builder.Services.AddSingleton(typeof(IGenericRepository<,>), typeof(MemoryGenericRepository<,>));
builder.Services.AddSingleton<IQuizUserService, QuizUserService>();
builder.Services.AddSingleton<IQuizAdminService, QuizAdminService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.Seed();
app.Run();