using Microsoft.EntityFrameworkCore;
using PolicyCoverageApi.data;
using PolicyCoverageApi.interfaces;
using PolicyCoverageApi.repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddScoped<IUserAuth, UserAuthRepository>();
builder.Services.AddScoped<IUserPolicy, UserPolicyRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddDbContext<UserDbContext>(options =>
options.UseMySQL(builder.Configuration.GetConnectionString("LocalConnectionString")));
builder.Services.AddDbContext<PolicyDbContext>(options =>
options.UseMySQL(builder.Configuration.GetConnectionString("PolicyConnectionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
