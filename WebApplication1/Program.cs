using ForumBE.Helpers;
using ForumBE.Mappings;
using ForumBE.Middlewares;
using ForumBE.Models;
using ForumBE.Repositories.ActivitiesLog;
using ForumBE.Repositories.Attachments;
using ForumBE.Repositories.Bookmarks;
using ForumBE.Repositories.Categories;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Implementations;
using ForumBE.Repositories.Interfaces;
using ForumBE.Repositories.IUnitOfWork;
using ForumBE.Repositories.Likes;
using ForumBE.Repositories.Notifications;
using ForumBE.Repositories.Posts;
using ForumBE.Repositories.Reports;
using ForumBE.Repositories.Users;
using ForumBE.Services.ActivitiesLog;
using ForumBE.Services.Auth;
using ForumBE.Services.Bookmarks;
using ForumBE.Services.Category;
using ForumBE.Services.Comments;
using ForumBE.Services.Implementations;
using ForumBE.Services.Interfaces;
using ForumBE.Services.IPost;
using ForumBE.Services.Notifications;
using ForumBE.Services.Posts;
using ForumBE.Services.Statistics;
using ForumBE.Services.User;
using ForumBE.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter Jwt Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ForumDBConn"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireModerator", policy => policy.RequireRole("Moderator", "Admin"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});


builder.Services.AddSignalR();


builder.Services.AddAutoMapper(typeof(UserMappings));

builder.Services.AddScoped<ClaimContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IStatisticsService, StatisticsService>();


// scoped User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// scoped category
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// scoped post
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

// scoped comment
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

// scoped attachment
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

// scoped role
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// scoped activity log
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

// scoped bookmark
builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();

// scoped like
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();

builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

// scoped notification
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// scoped report
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

// scoped report status
builder.Services.AddScoped<IReportStatusRepository, ReportStatusRepository>();
builder.Services.AddScoped<IReportStatusService, ReportStatusService>();

// scoped warning
builder.Services.AddScoped<IWarningRepository, WarningRepository>();
builder.Services.AddScoped<IWarningService, WarningService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<NotificationHub>();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "",  // Điều này sẽ map thư mục wwwroot trực tiếp vào root path
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=86400");
    }
});

app.UseCors("AllowFrontend");


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<HandlingAuthentication>();

app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationhub");

app.UseMiddleware<HandlingValidation>();

app.UseMiddleware<HandlingException>();

app.MapControllers();




app.Run();
