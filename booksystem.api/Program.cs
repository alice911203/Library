using booksystem.api.Data;
using booksystem.api.Middlewares;
using booksystem.api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);//.net組態設定，有設定可以自動載入appsettings.json、環境變數以及 User Secrets 中的所有設定，會整合成鍵值對應物件
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Configuration 是框架提供給存取這個「已經建置好的鍵值字典」的介面
//GetConnectionString是C#做的擴充方法，他會呼叫WebApplication.CreateBuilder(args)蒐集到的資料，並以ConnectionString當母節點，()中字串當子節點去搜尋對應連線資訊
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(connectionString));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
// 讓 Swagger 產生器掛載我們的 XML 中文註解
builder.Services.AddOpenApi();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // 驗證發行者
            ValidIssuer = jwtIssuer,

            ValidateAudience = true, // 驗證使用在哪一個應用程式
            ValidAudience = jwtAudience,

            ValidateLifetime = true, // 驗證過期時間 (.net預設有 5 分鐘緩衝時間)

            ValidateIssuerSigningKey = true, // 驗證Key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
