namespace Hb.AuthServer.Core.Entity
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string CodeRefresh { get; set; }
        public DateTime Expiration { get; set; }

    }
}
