using Conduit.Images.DataAccess.Articles;
using Conduit.Images.DataAccess.Images.Models;
using Dapper.FluentMap;
using Dapper.FluentMap.Configuration;
using FluentMigrator.Runner;

namespace Conduit.Images.DataAccess.Extensions;

public class DataAccessLayerInitializer
{
    private readonly IMigrationRunner _migrationRunner;

    public DataAccessLayerInitializer(
        IMigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    public async Task InitializeAsync(bool runMigrations)
    {
        FluentMapper.Initialize(InitializeFluentMapper);
        if (runMigrations)
        {
            await Task.Run(_migrationRunner.MigrateUp);
        }
        await Task.CompletedTask;
    }

    private static void InitializeFluentMapper(FluentMapConfiguration configuration)
    {
        configuration.AddMap(new ArticleDbModel.EntityMap());
        configuration.AddMap(new ArticleImageDbModel.EntityMap());
    }
}
