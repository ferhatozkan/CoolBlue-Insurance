﻿using Insurance.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Api.Repository
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "InsuranceDb");
        }
        public DbSet<SurchargeRate> SurchargeRates { get; set; }   
    }
}
