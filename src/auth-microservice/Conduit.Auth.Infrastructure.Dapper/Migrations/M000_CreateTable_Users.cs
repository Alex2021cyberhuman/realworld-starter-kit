using Conduit.Auth.Infrastructure.Dapper.Users.Mappings;
using FluentMigrator;

namespace Conduit.Auth.Infrastructure.Dapper.Migrations
{
    [Migration(0)]
    public class M000_CreateTable_Users : Migration
    {
        public override void Up()
        {
            Create.Table(UsersColumns.TableName)
                .WithColumn("user_id")
                .AsGuid()
                .PrimaryKey()
                .WithColumn("username")
                .AsString(1000)
                .Unique()
                .WithColumn("email")
                .AsString(1000)
                .Unique()
                .WithColumn("password")
                .AsString(1000)
                .WithColumn("image")
                .AsString(1000)
                .WithColumn("bio")
                .AsString(1000);
        }

        public override void Down()
        {
            Delete.Table(UsersColumns.TableName);
        }
    }
}
