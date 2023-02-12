using System.Net;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Articles.PresentationLayer.Controllers;

[ApiController]
[Route("[controller]")]
public class TagsController : ControllerBase
{
    [HttpGet]
    [Produces(typeof(TagList.ResponseBody))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTags(
        [FromServices] ITagRepository repository,
        CancellationToken cancellationToken)
    {
        var response = await repository.GetTags(cancellationToken);
        return Ok(response.Body);
    }
}
