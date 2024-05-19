


using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
        
        builder.Services.AddAuthorization();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                new MySqlServerVersion(new Version(8, 0, 21))
));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            ;
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddControllers();
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

        app.MapControllers();

        // prefix the check with api

        RouteGroupBuilder baseApp = app.MapGroup("/api");

        baseApp.MapGet(pattern: "/check",(ClaimsPrincipal user) =>{
            if(user != null && user.Identity.IsAuthenticated){
                    // log user in
                    Console.WriteLine("User is authenticated" + user.Identity.Name);

                return Results.Ok("Authenticated");
            }
            return Results.Unauthorized();
        } );
        // app.MapIdentityApi<IdentityUser>();
        app.Run();
    }
}