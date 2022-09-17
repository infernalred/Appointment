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
                Id = 2, Title = "Маникюр", Description = "Профессиональная маникюр", DurationMinutes = 60,
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
            var master1 = await context.Masters.AsTracking().FirstAsync(x => x.ServiceId == 1);

            var dt = new DateTime(2022, 9, 12, 9, 30, 0, DateTimeKind.Utc);

            var step = 30;

            var slot1 = new TimeSlot
            {
                Id = 1, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt,
                End = dt.AddMinutes(step)
            };

            var slot2 = new TimeSlot
            {
                Id = 2, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt.AddMinutes(step += 30),
                End = dt.AddMinutes(step += 30)
            };

            var slot3 = new TimeSlot
            {
                Id = 3, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt.AddMinutes(step += 30),
                End = dt.AddMinutes(step += 30)
            };

            var slot4 = new TimeSlot
            {
                Id = 4, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt.AddMinutes(step += 30),
                End = dt.AddMinutes(step += 30)
            };

            var slot5 = new TimeSlot
            {
                Id = 5, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt.AddMinutes(step += 30),
                End = dt.AddMinutes(step += 30)
            };

            var slot6 = new TimeSlot
            {
                Id = 6, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt.AddMinutes(step += 30),
                End = dt.AddMinutes(step += 30)
            };

            var slot7 = new TimeSlot
            {
                Id = 7, Master = master1,
                DayOfWeek = dt.DayOfWeek,
                Start = dt.AddMinutes(step += 30),
                End = dt.AddMinutes(step += 30)
            };

            await context.TimeSlots.AddRangeAsync(slot1, slot2, slot3, slot4, slot5, slot6, slot7);
            await context.SaveChangesAsync();
        }
    }
}