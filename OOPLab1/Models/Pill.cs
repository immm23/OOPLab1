using System;
using System.Collections.Generic;

namespace OOPLab1.Models;

public partial class Pill
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Class { get; set; }

    public virtual PillClass ClassNavigation { get; set; } = null!;

    public virtual ICollection<PillsAndIlness> PillsAndIlnesses { get; } = new List<PillsAndIlness>();

    public virtual ICollection<PillsAndPharmasy> PillsAndPharmasies { get; } = new List<PillsAndPharmasy>();
}
