using BrockAllen.MembershipReboot;
using MembershipReboot.HotTowel.App_Start;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MembershipReboot.HotTowel
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UseRSAencryption();

            // Initialize the Membership Reboot Database will create if does not exist
            Database.SetInitializer<DefaultMembershipRebootDatabase>(new CreateDatabaseIfNotExists<DefaultMembershipRebootDatabase>());
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            AreaRegistration.RegisterAllAreas(RouteTable.Routes);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InitMembershipDatabase();

        }
        private void InitMembershipDatabase()
        {
            var svc = new UserAccountService(new MembershipRebootConfiguration(new DefaultUserAccountRepository(SecuritySettings.Instance.ConnectionStringName)));
            if (svc.GetByUsername("admin") == null)
            {
                var account = svc.CreateAccount("admin", "admin123", "someone@gmail.com");
                svc.VerifyAccount(account.VerificationKey);
                account.AddClaim(ClaimTypes.Role, "Administrator");
                svc.Update(account);
            }
        }

        private void UseRSAencryption()
        {
            //Used to replace the DPAPI transforms (default) with one that uses RSA encryption using an X509 certificate
            // The service certificate is configured in the web.config <serviceCertificate> section. Needed to create the certificate or else it will be null.
            // Used to resolve the need for the loadUserProfile setting on the Application Pool to be set to true for DPAPI on a shared hosting server
            FederatedAuthentication.FederationConfigurationCreated += (sender, args) =>
            {
                var sessionTransforms = new List<CookieTransform>(new CookieTransform[]
            {
                new DeflateCookieTransform(),
                new RsaEncryptionCookieTransform(args.FederationConfiguration.ServiceCertificate),
                new RsaSignatureCookieTransform(args.FederationConfiguration.ServiceCertificate)
            });
                var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());
                args.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
            };
            // replace DPAPI transforms end.
        }
    }
}