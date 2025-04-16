#nullable disable
using System;
using System.Collections.Generic;

namespace PhotoFinder.Entity;

public partial class Availability
{
    public int AvailabilityId { get; set; }

    public int PhotographerId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Photographer Photographer { get; set; }
}