using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOPLab1.Models;

public partial class Pill
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Назва засобу не може бути порожньою")]
    public string Name { get; set; } = null!;

    public int Class { get; set; }

    [Display(Name = "Побічні ефекти")]
    public string? SideEffects { get; set; }

    [Display(Name = "Придатне до")]
    public DateTime? ExpiryDate { get; set; }

    [Display(Name = "Клас засобу")]
    public virtual Class? ClassNavigation { get; set; }

    [Display(Name = "Хвороби")]
    public virtual ICollection<Ilness> Illnes { get; } = new List<Ilness>();

    [Display(Name = "Аптеки")]
    public virtual ICollection<Pharmasy> Pharmasies { get; } = new List<Pharmasy>();


    [NotMapped]
    [Display(Name = "Хвороби")]
    public int[]? SelectedIllnes { get; set; }

    [NotMapped]
    [Display(Name = "Аптеки")]
    public int[]? SelectedPharmasies { get; set; }
}
