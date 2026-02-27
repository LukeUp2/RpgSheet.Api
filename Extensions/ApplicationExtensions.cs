using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgSheet.Api.Data;
using RpgSheet.Api.Data.Repositories;
using RpgSheet.Api.Infra.Storage;
using RpgSheet.Api.Security;
using RpgSheet.Api.UseCases.Sheet.Create;
using RpgSheet.Api.UseCases.Sheet.GetById;
using RpgSheet.Api.UseCases.Sheet.ListAll;
using RpgSheet.Api.UseCases.Sheet.UpdateVitals;
using RpgSheet.Api.UseCases.Sheet.UploadPortrait;
using RpgSheet.Api.UseCases.Skill.Create;

namespace RpgSheet.Api.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddUseCases(services);
            AddRepositories(services);
            AddUnitOfWork(services);
            AddGMValidator(services);
            AddSupabaseStorage(services);
        }

        private static void AddSupabaseStorage(IServiceCollection services)
        {
            services.AddSingleton<SupabaseStorage>();
        }


        private static void AddUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<UnitOfWork>();
        }

        private static void AddGMValidator(IServiceCollection services)
        {
            services.AddScoped<IGMKeyValidator, GMKeyValidator>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<SheetRepository>();
            services.AddScoped<SkillRepository>();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<CreateSheetUseCase>();
            services.AddScoped<GetSheetByIdUseCase>();
            services.AddScoped<ListSheetsForGMUseCase>();
            services.AddScoped<UpdateVitalsUseCase>();

            services.AddScoped<AddSkillUseCase>();

            services.AddScoped<UploadPortraitUseCase>();

        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}