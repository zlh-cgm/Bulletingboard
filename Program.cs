using Bulletingboard.DAO.Comment;
using Bulletingboard.DAO.Post;
using Bulletingboard.DAO.User;
using Bulletingboard.Entity;
using Bulletingboard.FluentValidators;
using Bulletingboard.Services.Auth;
using Bulletingboard.Services.Comment;
using Bulletingboard.Services.Mail;
using Bulletingboard.Services.Post;
using Bulletingboard.Services.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using UserEntity = Bulletingboard.Entity.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PostRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ResetPasswordRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ForgotPasswordRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordRequestValidator>();


//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var conStrBuilder = new MySqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("DefaultConnection"));

conStrBuilder.Password = builder.Configuration["DbPassword"];
var connectionString = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<BulletingboardDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddScoped<IUserDao, UserDao>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostDao, PostDao>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentDao, CommentDao>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BulletingboardDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}");

app.Run();
