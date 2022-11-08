using AppointmentService.Application.Contracts;
using AppointmentService.Application.Helpers;
using AppointmentService.Application.TimeSlots;
using AppointmentService.Domain;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppointmentService.Application.Tests.TimeSlotsTests;

public class UpdateTimeSlotTests
{
    private readonly MapperConfiguration _mapperConfiguration = new(cfg => cfg.AddProfile(new MappingProfiles()));

    [Fact]
    public async Task EditSlot_Success_FreeDay()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = DayOfWeek.Friday,
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddMinutes(180)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = DayOfWeek.Friday,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddMinutes(280)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.True(actual.IsSuccess);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task EditSlot_Success_StartExistEndTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var dt = new DateTime(2022, 9, 12, 8, 30, 0, DateTimeKind.Utc);
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = dt.DayOfWeek,
            Start = dt,
            End = dt.AddMinutes(60)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = dt.DayOfWeek,
                Start = dt,
                End = dt.AddMinutes(45)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.True(actual.IsSuccess);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task EditSlot_Success_EndExistStartTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var dt = new DateTime(2022, 9, 12, 9, 30, 0, DateTimeKind.Utc);
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = dt.DayOfWeek,
            Start = new DateTime(2022, 9, 12, 8, 30, 0, DateTimeKind.Utc),
            End = dt
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = dt.DayOfWeek,
                Start = new DateTime(2022, 9, 12, 8, 40, 0, DateTimeKind.Utc),
                End = dt
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.True(actual.IsSuccess);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task CreateSlot_Success_BetweenFreeExistTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = DayOfWeek.Monday,
            Start = new DateTime(2022, 9, 12, 8, 45, 0, DateTimeKind.Utc),
            End = new DateTime(2022, 9, 12, 9, 15, 0, DateTimeKind.Utc)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = DayOfWeek.Monday,
                Start = new DateTime(2022, 9, 12, 8, 33, 0, DateTimeKind.Utc),
                End = new DateTime(2022, 9, 12, 9, 25, 0, DateTimeKind.Utc)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.True(actual.IsSuccess);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task CreateSlot_Failure_IsExistTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var dt = new DateTime(2022, 9, 12, 3, 30, 0, DateTimeKind.Utc);
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = dt.DayOfWeek,
            Start = dt,
            End = dt.AddMinutes(60)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = DayOfWeek.Monday,
                Start = dt,
                End = DateTime.UtcNow.AddMinutes(180)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.False(actual.IsSuccess);
        Assert.Equal("Время не должно пересекаться", actual.Error);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task CreateSlot_Failure_BetweenIsExistTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var dt = new DateTime(2022, 9, 12, 16, 0, 0, DateTimeKind.Utc);
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = DayOfWeek.Friday,
            Start = dt,
            End = dt.AddMinutes(180)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = DayOfWeek.Monday,
                Start = new DateTime(2022, 9, 12, 6, 0, 0, DateTimeKind.Utc),
                End = new DateTime(2022, 9, 12, 8, 0, 0, DateTimeKind.Utc)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.False(actual.IsSuccess);
        Assert.Equal("Время не должно пересекаться", actual.Error);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task CreateSlot_Failure_StartIsExistTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var dt = new DateTime(2022, 9, 12, 16, 0, 0, DateTimeKind.Utc);
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = dt.DayOfWeek,
            Start = dt,
            End = dt.AddMinutes(180)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = dt.DayOfWeek,
                Start = new DateTime(2022, 9, 12, 5, 30, 0, DateTimeKind.Utc),
                End = new DateTime(2022, 9, 12, 8, 30, 0, DateTimeKind.Utc)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.False(actual.IsSuccess);
        Assert.Equal("Время не должно пересекаться", actual.Error);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task CreateSlot_Failure_EndIsExistTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var dt = new DateTime(2022, 9, 12, 4, 0, 0, DateTimeKind.Utc);
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = dt.DayOfWeek,
            Start = dt,
            End = dt.AddMinutes(80)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = dt.DayOfWeek,
                Start = dt,
                End = dt.AddMinutes(120)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);

        Assert.False(actual.IsSuccess);
        Assert.Equal("Время не должно пересекаться", actual.Error);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }

    [Fact]
    public async Task CreateSlot_Exception_NotExistMaster()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Edit.Handler>>();
        var mapper = new Mapper(_mapperConfiguration);
        var context = await SetupDbContext.Generate();
        var existSlot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            MasterId = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            DayOfWeek = DayOfWeek.Friday,
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddMinutes(180)
        };
        context.TimeSlots.Add(existSlot);
        await context.SaveChangesAsync(CancellationToken.None);

        var command = new Edit.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = existSlot.Id,
                DayOfWeek = DayOfWeek.Friday,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddMinutes(280)
            }
        };

        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master99");

        var handler = new Edit.Handler(logger.Object, context, mapper, mockUserAccessor.Object);

        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
            await handler.Handle(command, CancellationToken.None));
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
}