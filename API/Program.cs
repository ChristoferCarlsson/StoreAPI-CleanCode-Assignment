using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Application.Products.Commands.UpdateProduct;
using FluentValidation;
using Application.Products.Commands.CreateProduct;
using Application.Products.Queries.GetAllProducts;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Application.Products.Commands.DeleteProduct;
using Application.Categories.Commands.CreateCategory;
using Application.Categories.Queries.GetAllCategories;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Commands.DeleteCategory;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


            // Add Validators
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetAllProductsQueryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<DeleteProductCommandValidator>();

            // Category Validators
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetAllCategoriesQueryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<DeleteCategoryCommandValidator>();



            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // AutoMapper + MediatR
            builder.Services.AddAutoMapper(typeof(Application.Mappings.MappingProfile).Assembly);
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(Application.DTOs.ProductDto).Assembly)
                .RegisterServicesFromAssembly(typeof(Application.DTOs.CategoryDto).Assembly));


            // Add Controllers
            builder.Services.AddControllers();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();  // Needed for Swagger to work
            builder.Services.AddSwaggerGen();  // This adds Swagger services

            // Authentication & Authorization (JWT)
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // Set true if using HTTPS in production
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) // SecretKey from appsettings
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); // Enable Swagger in Development
                app.UseSwaggerUI(); // Enable Swagger UI
            }

            app.UseHttpsRedirection();

            // Use Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            app.Run();
        }
    }
}
