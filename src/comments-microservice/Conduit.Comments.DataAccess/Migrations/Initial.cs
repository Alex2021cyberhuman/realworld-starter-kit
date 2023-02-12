using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Comments.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article", x => x.article_id);
                    table.ForeignKey(
                        name: "fk_article_author_author_id",
                        column: x => x.author_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "author_follower",
                columns: table => new
                {
                    followeds_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followers_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author_follower", x => new { x.followeds_id, x.followers_id });
                    table.ForeignKey(
                        name: "fk_author_follower_author_followeds_id",
                        column: x => x.followeds_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_author_follower_author_followers_id",
                        column: x => x.followers_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comment", x => x.comment_id);
                    table.ForeignKey(
                        name: "fk_comment_article_article_id",
                        column: x => x.article_id,
                        principalTable: "article",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comment_author_author_id",
                        column: x => x.author_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_article_author_id",
                table: "article",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_slug",
                table: "article",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_author_username",
                table: "author",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_author_follower_followers_id",
                table: "author_follower",
                column: "followers_id");

            migrationBuilder.CreateIndex(
                name: "ix_comment_article_id",
                table: "comment",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "ix_comment_author_id",
                table: "comment",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_comment_created_at",
                table: "comment",
                column: "created_at");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "author_follower");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "author");
        }
    }
}
