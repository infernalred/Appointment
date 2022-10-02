using AppointmentService.Domain;
using AppointmentService.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Persistence;

public static class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var admin = new AppUser {DisplayName = "Admin", UserName = "admin", Email = "admin@test.com"};
            var manager1 = new AppUser {DisplayName = "Manager1", UserName = "manager1", Email = "manager1@test.com"};

            var master1 = new AppUser {DisplayName = "Master1", UserName = "master1", Email = "master1@test.com"};
            var master2 = new AppUser {DisplayName = "Master2", UserName = "master2", Email = "master2@test.com"};
            var master3 = new AppUser {DisplayName = "Master3", UserName = "master3", Email = "master3@test.com"};

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.CreateAsync(manager1, "Pa$$w0rd");
            await userManager.CreateAsync(master1, "Pa$$w0rd");
            await userManager.CreateAsync(master2, "Pa$$w0rd");
            await userManager.CreateAsync(master3, "Pa$$w0rd");
        }

        if (!context.Services.Any())
        {
            var service1 = new Service
            {
                Id = 1, Title = "Стрижка", Description = "Профессиональная стрижка", DurationMinutes = 30,
                IsEnabled = true
            };

            var service2 = new Service
            {
                Id = 2, Title = "Маникюр", Description = "Профессиональный маникюр", DurationMinutes = 60,
                IsEnabled = true
            };

            var service3 = new Service
            {
                Id = 3, Title = "Чистка лица", Description = "Профессиональная чистка", DurationMinutes = 90,
                IsEnabled = true
            };

            await context.Services.AddRangeAsync(service1, service2, service3);
            await context.SaveChangesAsync();
        }

        if (!context.Masters.Any())
        {
            var user1 = await userManager.Users.AsTracking().FirstAsync(x => x.UserName == "master1");
            var user2 = await userManager.Users.AsTracking().FirstAsync(x => x.UserName == "master2");
            var user3 = await userManager.Users.AsTracking().FirstAsync(x => x.UserName == "master3");

            var service1 = await context.Services.AsTracking().FirstAsync(x => x.Id == 1);
            var service2 = await context.Services.AsTracking().FirstAsync(x => x.Id == 2);
            var service3 = await context.Services.AsTracking().FirstAsync(x => x.Id == 3);

            var master1 = new Master {User = user1, Service = service1};
            var master2 = new Master {User = user2, Service = service2};
            var master3 = new Master {User = user3, Service = service3};

            await context.Masters.AddRangeAsync(master1, master2, master3);
            await context.SaveChangesAsync();
        }

        if (!context.TimeSlots.Any())
        {
            var master1 = await context.Masters
                .Include(x => x.Service)
                .AsTracking()
                .FirstAsync(x => x.ServiceId == 1);

            var dt = new DateTime(2022, 9, 12, 5, 30, 0, DateTimeKind.Utc);

            var step = master1.Service.DurationMinutes;

            var id = 1;

            var slots = new List<TimeSlot>();

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 12 * step; j += step)
                {
                    var slot = new TimeSlot
                    {
                        Id = id++, Master = master1,
                        DayOfWeek = dt.DayOfWeek,
                        Start = dt.AddMinutes(j),
                        End = dt.AddMinutes(j + step)
                    };
 
                    slots.Add(slot);
                }

                dt = dt.AddDays(1);
            }

            await context.TimeSlots.AddRangeAsync(slots);
            await context.SaveChangesAsync();
        }
    }
}