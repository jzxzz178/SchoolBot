using System.ComponentModel.DataAnnotations;

namespace SchoolBot.DbWork.Logic;

public class Error
{
    // public Guid id { get; set; }
    [Key] public DateTime Timestamp { get; set; }
    public string? Message { get; set; }
}