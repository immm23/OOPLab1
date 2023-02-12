using System;
using System.Collections.Generic;

namespace OOPLab1.Models;

public partial class PillsAndPharmasy
{
    public int PharmasyId { get; set; }

    public int PillId { get; set; }

    public int Id { get; set; }

    public virtual Pharmasy Pharmasy { get; set; } = null!;

    public virtual Pill Pill { get; set; } = null!;
}
