using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.WebApi;
using Newtonsoft.Json.Linq;
using BrockAllen.MembershipReboot;

namespace MembershipReboot.HotTowel.Controllers
{
    [BreezeController]
    public class BreezeController : ApiController
    {
        readonly EFContextProvider<DefaultMembershipRebootDatabase> _contextProvider =
            new EFContextProvider<DefaultMembershipRebootDatabase>();


        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        [HttpGet]
        public object Lookups()
        {
            var users =  _contextProvider.Context.Users;
            return new {users};
        }

        [HttpGet]
        public IQueryable<UserAccount> Users()
        {
            return _contextProvider.Context.Users;
        }

   }
}