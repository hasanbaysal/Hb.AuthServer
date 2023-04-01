using Hb.AuthServer.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hb.AuthServer.Data.Configurations
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {

            builder.HasKey(x => x.UserId);
            builder.Property(x=>x.CodeRefresh).IsRequired();

        }
    }








}
