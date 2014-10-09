using System;
using System.Collections.Generic;
using Mozu.Api.Contracts.AppDev;
using Mozu.Api.Contracts.Core;
using Mozu.Api.Contracts.Tenant;
using Mozu.Api.Resources.Platform;
using Mozu.Api.Security;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MozuImport
{
    public class Account
    {
        public static AuthenticationProfile Login(string emailAddress, string password)
        {
            var userAuthInfo = new UserAuthInfo { EmailAddress = emailAddress, Password = password };
            var userInfo = UserAuthenticator.Authenticate(userAuthInfo, AuthenticationScope.Tenant);
            return userInfo;
        }

        public static Dictionary<Int32, Scope> GetSandboxes(AuthenticationProfile authenticationProfile)
        {
            var sandboxes = new Dictionary<Int32, Scope>();
            var sbCount = 0;
            foreach (var sandbox in authenticationProfile.AuthorizedScopes)
            {
                sandboxes.Add(++sbCount, sandbox);
            }
            return sandboxes;
        }

        public static Tenant GetTenant(int sandBoxId)
        {
            var tenantResource = new TenantResource();
            return tenantResource.GetTenant(sandBoxId);
        }

    }
}
