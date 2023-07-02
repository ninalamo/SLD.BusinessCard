using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.Domain.Seedwork
{
    public interface ICurrentUser
    {
        string Name { get; }
        string Email { get; }
        string IdentityId { get; }
        string[] Roles { get; }


        bool IsAdmin();
    }
}
