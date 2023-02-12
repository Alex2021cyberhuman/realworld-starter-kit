using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Articles.DataAccessLayer.Models;

public class TagDbModelConfiguration : IEntityTypeConfiguration<TagDbModel>
{
    public void Configure(
        EntityTypeBuilder<TagDbModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("tag_id");
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasMany(x => x.Articles).WithMany(x => x.Tags)
            .UsingEntity(x => x.ToTable("tag_article"));
    }
}
