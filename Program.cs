


using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
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
    private static  void Main(string[] args)
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
            options.UseLazyLoadingProxies().UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21))));

        builder.Services.AddIdentity<UserModel, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<UserModel>>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

                builder.Services.ConfigureOptions<JwtOptionsSetup>();
        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
          
        builder.Services.AddAuthentication(
            options =>
            {
 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        
                }

        ).AddJwtBearer(
                options =>
                {
                     options.SaveToken = true;
                     options.RequireHttpsMetadata = false;
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                         ValidAudience = builder.Configuration[key: "JwtSettings:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
                     };
    //                 options.Events = new JwtBearerEvents
    // {
    //     OnAuthenticationFailed = context =>
    //     {
    //         Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
    //         return Task.CompletedTask;
    //     },
    //     OnTokenValidated = context =>
    //     {
    //         Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
    //         return Task.CompletedTask;
    //     }
    // };
    }
                      
        );  
        
      
            // .AddSignInManager<SignInManager<UserModel>>()
            // .AddRoles<IdentityRole>()
            // .AddRoleManager<RoleManager<IdentityRole>>()
      


     
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
).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.WriteIndented = true;
            // options.JsonSerializerOptions.PropertyNamingPolicy = null;



            //fuck you
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        


        });


        // builder.Services.AddLogging();

  
        

        builder.Services.AddAuthorization(
            options =>
            {

                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
                options.AddPolicy("Seller", policy => policy.RequireClaim(ClaimTypes.Role, "Seller"));
            }
        );



        WebApplication? app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            SeedRolesAsync(serviceProvider).Wait();
        }
        // app.UseStaticFiles();

        /**     before app run   **/
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseDeveloperExceptionPage();
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapIdentityApi<UserModel>();
        app.Run();
    }

    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User", "Seller"};

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

}

