﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DesignTime
{
    /// <summary>
    ///     Used for design time migrations.  
    ///     Will look to the appsettings.json file in this project for the connection string.
    ///     EF Core tools scans the assembly containing the dbcontext for an implementation
    ///     of IDesignTimeDbContextFactory.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            //Console.WriteLine($"DesignTimeDbContextFactory: using base path = {path}");
            //Console.WriteLine($"DesignTimeDbContextFactory: using connection string = {connectionString}");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find connection string named 'DefaultConnection'");
            }

            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<ApplicationDbContext>();

            ApplicationDbContext.AddBaseOptions(dbContextOptionsBuilder, connectionString);

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
