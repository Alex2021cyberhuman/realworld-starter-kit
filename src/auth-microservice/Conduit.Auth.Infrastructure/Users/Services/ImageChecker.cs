using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users.Services;
using SixLabors.ImageSharp;

namespace Conduit.Auth.Infrastructure.Users.Services
{
    public class ImageChecker : IImageChecker
    {
        private readonly HttpClient _client;

        public ImageChecker(HttpClient client)
        {
            _client = client;
        }

        #region IImageChecker Members

        async Task<bool> IImageChecker.CheckImageAsync(
            string url,
            CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(message, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return false;
            await using var stream =
                await response.Content.ReadAsStreamAsync(cancellationToken);
            try
            {
                var (image, _) = await Image.LoadWithFormatAsync(
                    Configuration.Default,
                    stream,
                    cancellationToken);
                return CheckSize(image);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        private static bool CheckSize(IImageInfo image)
        {
            return image.Width > 100 &&
                   image.Height > 100 &&
                   image.Width < 500 &&
                   image.Height < 500;
        }
    }
}
