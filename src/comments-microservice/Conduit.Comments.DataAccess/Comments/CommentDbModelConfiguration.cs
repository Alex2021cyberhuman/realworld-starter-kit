using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Conduit.Comments.DataAccess.Comments;

public class
    CommentDbModelConfiguration : IEntityTypeConfiguration<CommentDbModel>
{
    public void Configure(
        EntityTypeBuilder<CommentDbModel> builder)
    {
        builder.HasIndex(x => x.CreatedAt);
        builder.Property(x => x.Id).HasColumnName("comment_id");
        builder.Property(x => x.CreatedAt).HasValueGenerator((
            _,
            _) => DateTimeGenerator.Instance);
        builder.Property(x => x.UpdatedAt).HasValueGenerator((
            _,
            _) => DateTimeGenerator.Instance);

        builder.Navigation(x => x.Article).AutoInclude();
        builder.Navigation(x => x.Author).AutoInclude();
    }

    public class DateTimeGenerator : ValueGenerator<DateTime>
    {
        public static DateTimeGenerator Instance { get; } = new();

        public override bool GeneratesTemporaryValues { get; } = false;

        public override DateTime Next(
            EntityEntry entry)
        {
            return DateTime.UtcNow;
        }
    }
}
