using Conduit.Images.BusinessLogic.Images.AssignArticleImage;
using Conduit.Images.BusinessLogic.Images.RemoveArticleImage;
using Conduit.Images.BusinessLogic.Images.RemoveUnassignedImages;
using Conduit.Images.BusinessLogic.Images.UploadArticleImage;
using Conduit.Images.Domain.Images.AssignArticleImage;
using Conduit.Images.Domain.Images.RemoveArticleImage;
using Conduit.Images.Domain.Images.RemoveUnassignedImages;
using Conduit.Images.Domain.Images.UploadArticleImage;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Images.BusinessLogic.Extensions;

public static class BusinessLogicRegistrationExtensions
{
    public static void RegisterBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IAssignArticleImageHandler, AssignArticleImageHandler>()
            .AddScoped<IRemoveArticleImageHandler, RemoveArticleImageHandler>()
            .AddScoped<IRemoveUnassignedImagesHandler, RemoveUnassignedImagesHandler>()
            .AddScoped<IUploadArticleImageRequestHandler, UploadArticleImageRequestHandler>();
    }
}
