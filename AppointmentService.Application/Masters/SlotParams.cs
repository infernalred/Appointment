﻿namespace AppointmentService.Application.Masters;

public class SlotParams
{
    public DateTime Start { get; set; } = DateTime.UtcNow;
    public int QuantityDays { get; set; } = 6;
}