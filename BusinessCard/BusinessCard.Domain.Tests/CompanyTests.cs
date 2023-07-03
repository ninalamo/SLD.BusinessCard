using BusinessCard.Domain.AggregatesModel.CompanyAggregate;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.Domain.Tests
{
    public class CompanyTests
    {
        [Fact]
        public void shouldBeAbleToCreate()
        {
            Company company = new("ABS-CBN");
            company.Name.ShouldBe("ABS-CBN");
        }

        [Fact]
        public void shouldBeAbleToRename()
        {
            Company company = new("ABS-CBN");
            company.Name = "GMA";
            company.Name.ShouldBe("GMA");
        }
    }
}
