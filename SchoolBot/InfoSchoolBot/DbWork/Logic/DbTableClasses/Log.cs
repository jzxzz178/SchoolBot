﻿using System.ComponentModel.DataAnnotations;

namespace SchoolBot.DbWork.Logic.DbTableClasses;

public class Log
{
    [Key] public DateTime Timestamp { get; set; }
    public string? UserId { get; set; }
    public string? RequestType { get; set; }
}