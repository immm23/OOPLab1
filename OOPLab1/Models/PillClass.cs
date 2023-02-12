using System;
using System.Collections.Generic;

namespace OOPLab1.Models;

public partial class PillClass
{
    public string Name { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<Pill> Pills { get; } = new List<Pill>();
}
