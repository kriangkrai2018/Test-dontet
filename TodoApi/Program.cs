var builder = WebApplication.CreateBuilder(args);

// Add services to the container and register application layers.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<TodoApi.Services.ITaskService, TodoApi.Services.TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
