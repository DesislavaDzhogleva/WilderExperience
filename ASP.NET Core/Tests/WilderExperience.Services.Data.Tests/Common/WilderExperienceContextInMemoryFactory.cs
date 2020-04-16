using Microsoft.EntityFrameworkCore;
using System;
using WilderExperience.Data;

namespace WilderExperience.Services.Data.Tests.Common
{
    public class WilderExperienceContextInMemoryFactory
    {


        public static ApplicationDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
