﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.DbWork.Logic.DbCommunicators
{
    public sealed class DbTablesContext : AbstractDbTablesContext
    {
        private readonly IConfiguration? config;

        public DbTablesContext(IConfiguration? config = null)
        {
            this.config = config;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(config!.GetValue<string>("DATABASE_SOURCE"));
        }
    }
}