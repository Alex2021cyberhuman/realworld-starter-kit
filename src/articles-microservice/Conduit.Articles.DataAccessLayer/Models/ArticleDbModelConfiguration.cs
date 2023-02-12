using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Articles.DataAccessLayer.Models;

public class
    ArticleDbModelConfiguration : IEntityTypeConfiguration<ArticleDbModel>
{
    public void Configure(
        EntityTypeBuilder<ArticleDbModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => x.CreatedAt);
        builder.HasMany(x => x.Favoriters).WithMany(x => x.Favorites)
            .UsingEntity(x => x.ToTable("author_favorite"));
        builder.HasOne(x => x.Author).WithMany(x => x.Articles)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Id).HasColumnName("article_id");
    }
}
