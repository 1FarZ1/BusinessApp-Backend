


using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            c =>
            {
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
                new MySqlServerVersion(new Version(8, 0, 21))));

        builder.Services.AddAuthentication(
            options =>
            {
                        options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                        options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
                        options.DefaultSignInScheme = IdentityConstants.BearerScheme;
                        options.DefaultSignOutScheme = IdentityConstants.BearerScheme;
                        options.DefaultScheme = IdentityConstants.BearerScheme;
                        options.DefaultForbidScheme = IdentityConstants.BearerScheme;
                        
                }

        ).AddJwtBearer();
        
        builder.Services.ConfigureOptions<JwtOptionsSetup>();
        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
        builder.Services.AddIdentity<UserModel, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<UserModel>>()
            .AddSignInManager<SignInManager<UserModel>>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();
      


     
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IJwtService, JwtService>();

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
        // builder.Services.AddLogging();

  
        

        builder.Services.AddAuthorization(
            options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
            }
        );



        WebApplication? app = builder.Build();

        /**     before app run   **/
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // app.UseStaticFiles();
        // app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors("AllowAll");

        app.MapIdentityApi<UserModel>();
        app.Run();
    }
}