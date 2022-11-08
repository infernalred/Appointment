using AppointmentService.Domain;
using AppointmentService.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Persistence;

public static class Seed
{
    public static async Task SeedUser(DataContext context, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        const string adminRole = "Admin";
        const string managerRole = "Manager";
        const string masterRole = "Master";

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
            await roleManager.CreateAsync(new IdentityRole(managerRole));
            await roleManager.CreateAsync(new IdentityRole(masterRole));

            await context.SaveChangesAsync();
        }

        if (!userManager.Users.Any())
        {
            var admin = new AppUser {DisplayName = "Admin", UserName = "admin", Email = "admin@test.com"};
            var manager1 = new AppUser {DisplayName = "Manager1", UserName = "manager1", Email = "manager1@test.com"};

            var master1 = new AppUser {Id = "745bf0a5-9e0d-433d-afb4-a15b8e630d19", DisplayName = "Master1", UserName = "master1", Email = "master1@test.com"};
            var master2 = new AppUser {Id = "8b8eb53e-59ab-4a9b-9ae7-2fca51dc7494", DisplayName = "Master2", UserName = "master2", Email = "master2@test.com"};
            var master3 = new AppUser {Id = "5d15e1d8-6fb9-48a8-b941-7faf4815f24c", DisplayName = "Master3", UserName = "master3", Email = "master3@test.com"};

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.CreateAsync(manager1, "Pa$$w0rd");
            await userManager.CreateAsync(master1, "Pa$$w0rd");
            await userManager.CreateAsync(master2, "Pa$$w0rd");
            await userManager.CreateAsync(master3, "Pa$$w0rd");

            await context.SaveChangesAsync();

            await userManager.AddToRoleAsync(admin, adminRole);
            await userManager.AddToRoleAsync(admin, managerRole);
            await userManager.AddToRoleAsync(manager1, managerRole);
            await userManager.AddToRoleAsync(master1, masterRole);
            await userManager.AddToRoleAsync(master2, masterRole);
            await userManager.AddToRoleAsync(master3, masterRole);

            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedData(DataContext context)
    {
        if (!context.Services.Any())
        {
            var service1 = new Service
            {
                Id = Guid.Parse("cc1149bc-3e38-4e11-8496-0db1d683ae22"), Title = "Стрижка",
                Description = "Профессиональная стрижка", DurationMinutes = 30,
                IsEnabled = true
            };

            var service2 = new Service
            {
                Id = Guid.Parse("a92c56ee-19e3-471f-9c70-bad22f6fcb1f"), Title = "Маникюр",
                Description = "Профессиональный маникюр", DurationMinutes = 60,
                IsEnabled = true
            };

            var service3 = new Service
            {
                Id = Guid.Parse("b6db5450-27bf-48ad-9bac-da3b911d6ce4"), Title = "Чистка лица",
                Description = "Профессиональная чистка", DurationMinutes = 90,
                IsEnabled = true
            };

            await context.Services.AddRangeAsync(service1, service2, service3);
            await context.SaveChangesAsync();
        }

        if (!context.Masters.Any())
        {
            var service1 = await context.Services.AsTracking()
                .FirstAsync(x => x.Id == Guid.Parse("cc1149bc-3e38-4e11-8496-0db1d683ae22"));
            var service2 = await context.Services.AsTracking()
                .FirstAsync(x => x.Id == Guid.Parse("a92c56ee-19e3-471f-9c70-bad22f6fcb1f"));
            var service3 = await context.Services.AsTracking()
                .FirstAsync(x => x.Id == Guid.Parse("b6db5450-27bf-48ad-9bac-da3b911d6ce4"));

            var master1 = new Master {Id = "745bf0a5-9e0d-433d-afb4-a15b8e630d19", Service = service1};
            var master2 = new Master {Id = "8b8eb53e-59ab-4a9b-9ae7-2fca51dc7494", Service = service2};
            var master3 = new Master {Id = "5d15e1d8-6fb9-48a8-b941-7faf4815f24c", Service = service3};

            await context.Masters.AddRangeAsync(master1, master2, master3);
            await context.SaveChangesAsync();
        }

        if (!context.TimeSlots.Any())
        {
            var master1 = await context.Masters
                .Include(x => x.Service)
                .AsTracking()
                .FirstAsync(x => x.ServiceId == Guid.Parse("cc1149bc-3e38-4e11-8496-0db1d683ae22"));

            var dt = new DateTime(2022, 9, 12, 5, 30, 0, DateTimeKind.Utc);

            var slots = new List<TimeSlot>();

            for (var i = 0; i < 4; i++)
            {
                var slot1 = new TimeSlot
                {
                    Id = Guid.NewGuid(), Master = master1,
                    DayOfWeek = dt.DayOfWeek,
                    Start = dt,
                    End = dt.AddMinutes(180)
                };
                var slot2 = new TimeSlot
                {
                    Id = Guid.NewGuid(), Master = master1,
                    DayOfWeek = dt.DayOfWeek,
                    Start = dt.AddMinutes(240),
                    End = dt.AddMinutes(420)
                };

                slots.Add(slot1);
                slots.Add(slot2);

                dt = dt.AddDays(1);
            }

            await context.TimeSlots.AddRangeAsync(slots);
            await context.SaveChangesAsync();
        }
    }
}