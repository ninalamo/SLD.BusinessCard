using BusinessCard.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.Domain.AggregatesModel.CompanyAggregate
{
    public class Company : Entity, IAggregateRoot
    {
        public string Name { get; set; }

        public Company(string name)
        {
            Name = name;   
        }
        public static Company Create(string name) => new(name);
    }
}
