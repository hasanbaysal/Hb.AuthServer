using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Common.Options
{
    public class CustomTokenOptions
    {
        public List<string> Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public int AccessTokenExpiration { get; set; } 
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; } = null!;
    }
}
