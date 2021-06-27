
using BehinRahkar.Catalog.Api.Middlewares;
using BehinRahkar.Catalog.Application.Product.Queries;
using BehinRahkar.Catalog.Infra.Config;
using BehinRahkar.Catalog.Infra.Data;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using BehinRahkar.Catalog.Application.Product.Commands;
using BehinRahkar.Catalog.Application.AutoMapper;
using BehinRahkar.Application.Contracts;

namespace BehinRahkar.Catalog.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //---Mediator--------------------------------------------------------
            services.AddMediatR(typeof(GetAllProductQuery));
            //-------------------------------------------------------------------

            //---FluentValidation------------------------------------------------
            services.AddControllers(opt=> opt.Filters.Add(typeof(JsonExceptionFilter))).AddFluentValidation(s =>
            {
                s.RegisterValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
            });
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            //-------------------------------------------------------------------

            //---TraceBehavior---------------------------------------------------
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TraceBehavior<,>));
            //-------------------------------------------------------------------

            //---Add Versioning--------------------------------------------------
            services.AddApiVersioning(options => {options.ReportApiVersions = true;});
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";
                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
            //-------------------------------------------------------------------


            //---DbContext-------------------------------------------------------
            services.AddScoped<ICatalogDbContext, CatalogDbContext>();
            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CatalogDbConnection"));
            });
            //-------------------------------------------------------------------

            
            //---AutoMapper------------------------------------------------------
            services.AddAutoMapper(typeof(DomaintoDtoMappingProfile));
            //-------------------------------------------------------------------


            //---Swagger---------------------------------------------------------
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            //-------------------------------------------------------------------




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //---Swagger---------------------------------------------------------
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            //-------------------------------------------------------------------
        }
    }
}
