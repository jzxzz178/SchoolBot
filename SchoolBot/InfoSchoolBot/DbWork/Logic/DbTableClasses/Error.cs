using System.ComponentModel.DataAnnotations;

namespace SchoolBot.DbWork.Logic.DbTableClasses;

public class Error
{
    [Key] public DateTime Timestamp { get; set; }
    public string? Message { get; set; }
}