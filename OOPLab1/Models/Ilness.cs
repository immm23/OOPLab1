using System;
using System.Collections.Generic;

namespace OOPLab1.Models;

public partial class Ilness
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Pills { get; set; }

    public int Id { get; set; }

    public virtual ICollection<PillsAndIlness> PillsAndIlnesses { get; } = new List<PillsAndIlness>();
}
