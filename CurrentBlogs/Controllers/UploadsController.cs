using CurrentBlogs.Data;
using CurrentBlogs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CurrentBlogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //DI
    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60)]
        public async Task<IActionResult> GetImage(Guid id)
        {

            ImageUpload? image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);
            //Turnery Statement
            return image == null ? NotFound() : File(image.Data!, image.Type!);

        }
    }
}
