using System.ComponentModel.DataAnnotations;

namespace SchoolBot.DbWork.Logic.DbTableClasses;

public class Error
{
    // public Guid id { get; set; }
    [Key] public DateTime Timestamp { get; set; }
    public string? Message { get; set; }
}