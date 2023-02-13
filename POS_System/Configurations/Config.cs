using BLL.Interfaces;
using BLL.Services;
using DataLayer.Context;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API.Configurations
{
    public static class Config
    {
        static readonly string CORSOpenPolicy = "OpenCORSPolicy";
        public static void AddServices(this WebApplicationBuilder builder)
        {
            //API Services
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add DI Services
            builder.Services.AddTransient<ICustomerInterface, CustomerRepository>();
            builder.Services.AddTransient<ILoanInterface, LoanRepository>();
            builder.Services.AddTransient<ILoanPaymentInterface, LoanPaymentRepository>();
            builder.Services.AddTransient<IProductInterface, ProductRepository>();
            builder.Services.AddTransient<IReceiptInterface, ReceiptRepository>();
            builder.Services.AddTransient<ITransactionInterface, TransactionRepository>();
            builder.Services.AddTransient<IWarehouseInterface, WarehouseRepository>();
            builder.Services.AddTransient<IWarehouseItemInterface, WarehouseItemRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<IWarehouseService, WarehouseService>();
            builder.Services.AddTransient<IProductService, ProductService>();

            //Add dbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("LocalDB")));

            #region auth
            ////Add Identity
            //builder.Services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<HotelDbContext>()
            //.AddDefaultTokenProviders();

            ////Add Authentication
            //var tokenParameters = new TokenValidationParameters()
            //{
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:securityKey"])),

            //    ValidateIssuer = true,
            //    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],

            //    ValidateAudience = true,
            //    ValidAudience = builder.Configuration["JwtSettings:Audence"],
            //    ValidateLifetime = true,
            //    ClockSkew = TimeSpan.Zero
            //};
            //builder.Services.AddSingleton(tokenParameters);
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = tokenParameters;
            //    });
            #endregion

            //Add cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                  name: CORSOpenPolicy,
                  builder => {
                      builder.WithOrigins("http://localhost:4200")
                      .AllowCredentials()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
                  });
            });
        }

        public static void AddMiddlewares(this WebApplication app)
        {
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
                app.UseSwagger();
                app.UseSwaggerUI();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors(CORSOpenPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
