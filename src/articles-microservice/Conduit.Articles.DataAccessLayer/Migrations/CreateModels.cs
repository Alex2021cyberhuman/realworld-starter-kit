#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Conduit.Articles.DataAccessLayer.Migrations;

public partial class CreateModels : Migration
{
    protected override void Up(
        MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("author",
            table => new
            {
                author_id = table.Column<Guid>("uuid", nullable: false),
                username = table.Column<string>("text", nullable: false),
                bio = table.Column<string>("text", nullable: true),
                image = table.Column<string>("text", nullable: true)
            }, constraints: table =>
            {
                table.PrimaryKey("pk_author", x => x.author_id);
            });

        migrationBuilder.CreateTable("tag",
            table => new
            {
                tag_id = table.Column<Guid>("uuid", nullable: false),
                name = table.Column<string>("text", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("pk_tag", x => x.tag_id);
            });

        migrationBuilder.CreateTable("article",
            table => new
            {
                article_id = table.Column<Guid>("uuid", nullable: false),
                slug = table.Column<string>("text", nullable: false),
                title = table.Column<string>("text", nullable: false),
                description = table.Column<string>("text", nullable: false),
                body = table.Column<string>("text", nullable: false),
                created_at =
                    table.Column<DateTime>("timestamp with time zone",
                        nullable: false),
                updated_at =
                    table.Column<DateTime>("timestamp with time zone",
                        nullable: false),
                favorites_count =
                    table.Column<int>("integer", nullable: false),
                author_id = table.Column<Guid>("uuid", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("pk_article", x => x.article_id);
                table.ForeignKey("fk_article_author_author_id",
                    x => x.author_id, "author", "author_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable("author_follower",
            table => new
            {
                followeds_id = table.Column<Guid>("uuid", nullable: false),
                followers_id = table.Column<Guid>("uuid", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("pk_author_follower",
                    x => new { x.followeds_id, x.followers_id });
                table.ForeignKey("fk_author_follower_author_followeds_id",
                    x => x.followeds_id, "author", "author_id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey("fk_author_follower_author_followers_id",
                    x => x.followers_id, "author", "author_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable("author_favorite",
            table => new
            {
                favoriters_id = table.Column<Guid>("uuid", nullable: false),
                favorites_id = table.Column<Guid>("uuid", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("pk_author_favorite",
                    x => new { x.favoriters_id, x.favorites_id });
                table.ForeignKey("fk_author_favorite_article_favorites_id",
                    x => x.favorites_id, "article", "article_id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey("fk_author_favorite_author_favoriters_id",
                    x => x.favoriters_id, "author", "author_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable("tag_article",
            table => new
            {
                articles_id = table.Column<Guid>("uuid", nullable: false),
                tags_id = table.Column<Guid>("uuid", nullable: false)
            }, constraints: table =>
            {
                table.PrimaryKey("pk_tag_article",
                    x => new { x.articles_id, x.tags_id });
                table.ForeignKey("fk_tag_article_article_articles_id",
                    x => x.articles_id, "article", "article_id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey("fk_tag_article_tag_tags_id", x => x.tags_id,
                    "tag", "tag_id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex("ix_article_author_id", "article",
            "author_id");

        migrationBuilder.CreateIndex("ix_article_created_at", "article",
            "created_at");

        migrationBuilder.CreateIndex("ix_article_slug", "article", "slug",
            unique: true);

        migrationBuilder.CreateIndex("ix_author_username", "author", "username",
            unique: true);

        migrationBuilder.CreateIndex("ix_author_favorite_favorites_id",
            "author_favorite", "favorites_id");

        migrationBuilder.CreateIndex("ix_author_follower_followers_id",
            "author_follower", "followers_id");

        migrationBuilder.CreateIndex("ix_tag_name", "tag", "name",
            unique: true);

        migrationBuilder.CreateIndex("ix_tag_article_tags_id", "tag_article",
            "tags_id");
    }

    protected override void Down(
        MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("author_favorite");

        migrationBuilder.DropTable("author_follower");

        migrationBuilder.DropTable("tag_article");

        migrationBuilder.DropTable("article");

        migrationBuilder.DropTable("tag");

        migrationBuilder.DropTable("author");
    }
}
