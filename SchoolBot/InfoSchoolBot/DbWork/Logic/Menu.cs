using System.ComponentModel.DataAnnotations;

namespace SchoolBot.DbWork.Logic;

public class Menu
{
    [Key] public string? Day { get; set; }
    public string? Breakfast { get; set; }
    public string? Lunch { get; set; }
}