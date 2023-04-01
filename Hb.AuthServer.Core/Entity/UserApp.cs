using Microsoft.AspNetCore.Identity;

namespace Hb.AuthServer.Core.Entity
{
    public class UserApp:IdentityUser
    {
        public string City { get; set; }
    }
}
