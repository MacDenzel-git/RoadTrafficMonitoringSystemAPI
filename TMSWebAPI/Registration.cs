using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TMSBusinessLogicLayer.Repository;
using TMSBusinessLogicLayer.Services;
using TMSBusinessLogicLayer.Services.PasswordRecover;
using TMSBusinessLogicLayer.Services.Position;
using TMSBusinessLogicLayer.Services.Roles;

using TMSDataAccessLayer.TMSDB;

namespace TMSWebAPI
{
    public static class Registration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection service)
        {
             service.AddScoped<TMSGenericRepository<DriverLicense>>();
            service.AddScoped<TMSGenericRepository<VehicleInsurance>>();
            service.AddScoped<TMSGenericRepository<Crimes>>();
            service.AddScoped<TMSGenericRepository<TrafficMonitorTransactions>>();
            service.AddScoped<TMSGenericRepository<Credentials>>();
            service.AddScoped<TMSGenericRepository<Roles>>();
            service.AddScoped<TMSGenericRepository<Branch>>();
            service.AddScoped<TMSGenericRepository<Positions>>();
            service.AddScoped<TMSGenericRepository<RecoveryData>>();
            return service.AddScoped<TMSGenericRepository<PersonalDetails>>();
        }

        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IAccountService, AccountService>();
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IBranchService, BranchService>();
            service.AddScoped<IPositionService, PositionService>();
            service.AddScoped<IVehicleInsuranceService, VehicleInsuranceService>();
            service.AddScoped<ICrimeService, CrimeService>();
            service.AddScoped<IDriverLicenseService, DriverLicenseService>();
            service.AddScoped<ICrimeMantainance, CrimeMantainance>();
            return service.AddScoped<IPasswordRecover, PasswordRecover>();
            //service.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

        }

    }
}
