using AppointmentService.Application.Contracts;
using AppointmentService.Application.TimeSlots;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppointmentService.Application.Tests.TimeSlotsTests;

public class CreateTimeSlotTests
{
    [Fact]
    public async Task CreateSlot_Success_FreeDay()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Friday,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddMinutes(180)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);
        
        Assert.True(actual.IsSuccess);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
    
    [Fact]
    public async Task CreateSlot_Success_StartExistEndTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Friday,
                Start = new DateTime(2022, 9, 12, 8, 30, 0, DateTimeKind.Utc),
                End = DateTime.UtcNow.AddMinutes(60)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);
        
        Assert.True(actual.IsSuccess);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
    
    [Fact]
    public async Task CreateSlot_Success_EndExistStartTime()
    {
        var mockUserAccessor = new Mock<IUserAccessor>();
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Friday,
                Start = new DateTime(2022, 9, 12, 8, 30, 0, DateTimeKind.Utc),
                End = new DateTime(2022, 9, 12, 9, 30, 0, DateTimeKind.Utc)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

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
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Friday,
                Start = new DateTime(2022, 9, 12, 8, 45, 0, DateTimeKind.Utc),
                End = new DateTime(2022, 9, 12, 9, 15, 0, DateTimeKind.Utc)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

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
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                Start = new DateTime(2022, 9, 12, 5, 30, 0, DateTimeKind.Utc),
                End = DateTime.UtcNow.AddMinutes(60)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

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
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                Start = new DateTime(2022, 9, 12, 5, 0, 0, DateTimeKind.Utc),
                End = DateTime.UtcNow.AddMinutes(200)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

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
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                Start = new DateTime(2022, 9, 12, 8, 0, 0, DateTimeKind.Utc),
                End = DateTime.UtcNow.AddMinutes(90)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

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
        var logger = new Mock<ILogger<Create.Handler>>();
        var context = await SetupDbContext.Generate();

        var command = new Create.Command
        {
            TimeSlot = new TimeSlotDto
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                Start = new DateTime(2022, 9, 12, 4, 0, 0, DateTimeKind.Utc),
                End = DateTime.UtcNow.AddMinutes(180)
            }
        };
        
        mockUserAccessor.Setup(x => x.GetUsername()).Returns("master1");

        var handler = new Create.Handler(logger.Object, context, mockUserAccessor.Object);

        var actual = await handler.Handle(command, CancellationToken.None);
        
        Assert.False(actual.IsSuccess);
        Assert.Equal("Время не должно пересекаться", actual.Error);
        mockUserAccessor.Verify(x => x.GetUsername());
        mockUserAccessor.VerifyNoOtherCalls();
        mockUserAccessor.VerifyAll();
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
}