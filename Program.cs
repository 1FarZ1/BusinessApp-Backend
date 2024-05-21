


using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyAPI;
using MySql.Data.EntityFrameworkCore;
internal class Program
{
    private static void Main(string[] args)
    {

        /**  Building **/
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(
            c=>{
c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
  c.SwaggerDoc(name: "v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Btp Win",
        Version = "v1",
        Description = "Btp Win API",
    });
            }
        );
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                new MySqlServerVersion(new Version(8, 0, 21))
));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            ;


        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(
            options =>
            {
             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                };
            });
        builder.Services.AddScoped<IAuthService, AuthService>();
        // allow all cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        builder.Services.AddControllers(
        //     opt => {
        //     var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
        //     opt.Filters.Add(new AuthorizeFilter(policy));
        // }
        );
        builder.Services.AddLogging();
        WebApplication? app = builder.Build();
        /**     before app run   **/
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // app.UseStaticFiles();
        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors("AllowAll");

        // RouteGroupBuilder baseApp = app.MapGroup(prefix: "/api");
        app.MapGet("/", () => Results.Ok("Hello World!"));
        app.MapGet(pattern: "/check",(ClaimsPrincipal user) =>{
        // return the user object
        return Results.Ok(user.Identity.Name);
        } ).RequireAuthorization();
        // app.MapIdentityApi<IdentityUser>();
        app.Run();
    }
}