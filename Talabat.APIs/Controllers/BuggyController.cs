using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    
    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("NotFound")]
        public ActionResult GetFoundRequest()
        {
            var Product = _dbcontext.Products.Find(100);
            if(Product is null) return NotFound(new ApiResponse(404));
            return Ok(Product);
        }


        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Product = _dbcontext.Products.Find(100);
            var ProductToRtuen = Product.ToString();
            return Ok(ProductToRtuen);

        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }




    }
}
