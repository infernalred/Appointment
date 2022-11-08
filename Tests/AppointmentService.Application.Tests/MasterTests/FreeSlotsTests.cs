using AppointmentService.Application.Masters;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppointmentService.Application.Tests.MasterTests;

public class FreeSlotsTests
{
    [Fact]
    public async Task FreeSlot_Success_Week()
    {
        var logger = new Mock<ILogger<FreeSlots.Handler>>();
        var context = await SetupDbContext.Generate();
        var dt = DateTime.UtcNow;

        var command = new FreeSlots.Query
        {
            Id = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            Params = new SlotParams
            {
                Start = dt.AddDays(7)
            }
        };

        var handler = new FreeSlots.Handler(logger.Object, context);

        var actual = await handler.Handle(command, CancellationToken.None);
        
        Assert.True(actual.IsSuccess);
        Assert.Equal(48, actual.Result?.Count);
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
    
    [Fact]
    public async Task FreeSlot_Success_Monday()
    {
        var logger = new Mock<ILogger<FreeSlots.Handler>>();
        var context = await SetupDbContext.Generate();
        var dt = DateTime.UtcNow;

        var command = new FreeSlots.Query
        {
            Id = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            Params = new SlotParams
            {
                Start = dt.AddDays(7)
            }
        };

        var handler = new FreeSlots.Handler(logger.Object, context);

        var actual = await handler.Handle(command, CancellationToken.None);
        
        Assert.True(actual.IsSuccess);
        Assert.Equal(12, actual.Result?.Where(x => x.Start.DayOfWeek == DayOfWeek.Monday).ToList().Count);
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
    
    [Fact]
    public async Task FreeSlot_Success_MondayCorrectTime()
    {
        var logger = new Mock<ILogger<FreeSlots.Handler>>();
        var context = await SetupDbContext.Generate();
        var dt = DateTime.UtcNow;

        var command = new FreeSlots.Query
        {
            Id = "745bf0a5-9e0d-433d-afb4-a15b8e630d19",
            Params = new SlotParams
            {
                Start = dt.AddDays(7)
            }
        };

        var handler = new FreeSlots.Handler(logger.Object, context);

        var actual = await handler.Handle(command, CancellationToken.None);

        var slots = actual.Result?.Where(x => x.Start.DayOfWeek == DayOfWeek.Monday).ToList();
        var first = slots?.FirstOrDefault();
        var last = slots?.LastOrDefault();
        
        Assert.True(actual.IsSuccess);
        
        Assert.Equal(12,slots?.Count);
        
        Assert.Equal(5, first?.Start.Hour);
        Assert.Equal(30, first?.Start.Minute);
        Assert.Equal(6, first?.End.Hour);
        Assert.Equal(0, first?.End.Minute);
        
        Assert.Equal(12, last?.Start.Hour);
        Assert.Equal(0, last?.Start.Minute);
        Assert.Equal(12, last?.End.Hour);
        Assert.Equal(30, last?.End.Minute);
        
        logger.VerifyNoOtherCalls();
        logger.VerifyAll();
    }
}