using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Comments.DataAccess.Authors;

public class
    AuthorDbModelConfiguration : IEntityTypeConfiguration<AuthorDbModel>
{
    public void Configure(
        EntityTypeBuilder<AuthorDbModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.Id).HasColumnName("author_id");
        builder.HasMany(x => x.Followers).WithMany(x => x.Followeds)
            .UsingEntity(x => x.ToTable("author_follower"));
    }
}
