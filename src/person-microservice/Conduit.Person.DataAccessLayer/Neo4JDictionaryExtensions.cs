using Conduit.Person.BusinessLogic;
using Conduit.Shared.Events.Models.Users;
using Neo4j.Driver;

namespace Conduit.Person.DataAccessLayer;

public static class Neo4JDictionaryExtensions
{
    public static IDictionary<string, object?> ToDictionary(
        this RegisterUserEventModel model)
    {
        return new Dictionary<string, object?>
        {
            ["Id"] = model.Id.ToString(),
            ["Username"] = model.Username,
            ["Email"] = model.Email,
            ["Image"] = model.Image,
            ["Biography"] = model.Biography
        };
    }

    public static IDictionary<string, object?> ToDictionary(
        this UpdateUserEventModel model)
    {
        return new Dictionary<string, object?>
        {
            ["Id"] = model.Id.ToString(),
            ["Username"] = model.Username,
            ["Email"] = model.Email,
            ["Image"] = model.Image,
            ["Biography"] = model.Biography
        };
    }

    public static IDictionary<string, object?> ToDictionary(
        this FollowingInfo model)
    {
        var (followedUsername, followerUserId) = model;
        return new Dictionary<string, object?>
        {
            ["FollowerUserId"] = GetId(followerUserId),
            ["FollowedUsername"] = followedUsername
        };
    }

    private static string GetId(
        Guid? followerUserId)
    {
        return followerUserId?.ToString() ?? string.Empty;
    }

    public static (ProfileResponse, Guid) ToProfileResponse(
        this IRecord profileRecord)
    {
        return (
            new(profileRecord["username"].As<string>(),
                profileRecord["image"].As<string>(),
                profileRecord["biography"].As<string>(),
                profileRecord["followed"].As<bool>()),
            Guid.Parse(profileRecord["id"].As<string>()));
    }
}
