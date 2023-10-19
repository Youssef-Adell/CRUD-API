using Microsoft.AspNetCore.Mvc;

using Core.Entities.LinkModels;

namespace Web.Controllers;

[Route("api")]
[ApiController]
public class RootController : Controller
    {
        private LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name ="GetRoot")]
        public IActionResult GetRoot()
        {
            List<Link> links = new List<Link>
            {
                new Link{
                    Href=_linkGenerator.GetPathByName(HttpContext, nameof(GetRoot)),
                    Rel="self",
                    Method="GET"
                },
                new Link{
                    Href=_linkGenerator.GetPathByName(HttpContext, "GetAllCompanies"),
                    Rel="get_companies",
                    Method="GET"
                },
                new Link{
                    Href=_linkGenerator.GetPathByName(HttpContext, "CreateCompany"),
                    Rel="create-company",
                    Method="POST"
                }
            };

            return Ok(links);
        }
    }
