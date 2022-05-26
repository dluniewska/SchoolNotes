using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using School.Authorization;
using School.DTO;
//using School.DTO.Validators;
using School.Helpers;
using School.Middleware;
using School.Models;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var authenticationSettings = new AuthenticationSettings();
            //Configuration.GetSection("Authentication").Bind(authenticationSettings);
            //services.AddSingleton(authenticationSettings);
            //services.AddAuthentication(option => 
            //{
            //    option.DefaultAuthenticateScheme = "Bearer";
            //    option.DefaultScheme= "Bearer";
            //    option.DefaultChallengeScheme= "Bearer";
            //}).AddJwtBearer(cfg => 
            //{
            //    cfg.RequireHttpsMetadata = false;
            //    cfg.SaveToken = true;
            //    cfg.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidIssuer = authenticationSettings.JwtIssuer,
            //        ValidAudience = authenticationSettings.JwtIssuer,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
            //    };
            //});
            //services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            services.AddControllers().AddNewtonsoftJson().AddFluentValidation(); ;
            services.AddDbContext<ApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("APIContext")));
            //services.AddScoped<RolesSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IFilesService, FilesService>();
            //services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            //services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();
            //services.AddTransient<TokenManagerMiddleware>();
            //services.AddTransient<ITokenManager, TokenManager>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School", Version = "v1" });
                c.ResolveConflictingActions(x => x.First());
                c.DocumentFilter<JsonPatchDocumentFilter>();
                //c.OperationFilter<FileOperationFilter>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCaching(); 
            //seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SchoolAPI"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();
            //app.UseMiddleware<TokenManagerMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
