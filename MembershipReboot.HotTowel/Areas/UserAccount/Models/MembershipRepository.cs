using System;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Security;
using System.Security.Principal;
using Breeze.WebApi;
using BrockAllen.MembershipReboot;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class MembershipRepository : EFContextProvider<MembershipRebootDbContext>
    {
        public MembershipRepository()
        {
        }


    }
}