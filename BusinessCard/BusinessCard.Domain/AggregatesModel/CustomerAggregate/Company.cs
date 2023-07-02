using BusinessCard.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.Domain.AggregatesModel.CustomerAggregate
{
    public class Company : Enumeration
    {
        public Company(Guid id, string name) : base(id, name)
        {
        }

        public static Company Create(string name) => new(Guid.NewGuid(), name);
    }
}
