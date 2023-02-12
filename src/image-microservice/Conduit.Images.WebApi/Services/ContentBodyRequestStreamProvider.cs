using Conduit.Images.Domain.Images.Services.Streams;

namespace Conduit.Images.WebApi.Services
{
    public class ContentBodyRequestStreamProvider : IRequestStreamProvider
    {
        private readonly HttpContext _context;

        public ContentBodyRequestStreamProvider(HttpContext context)
        {
            this._context = context;
        }

        public Task<Stream> ProvideStreamAsync()
        {
            var stream = _context.Request.Body;
            return Task.FromResult(stream);
        }
    }
}
