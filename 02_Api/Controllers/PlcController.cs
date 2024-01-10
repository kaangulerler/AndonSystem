using _01_DbModel.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _02_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlcController : ControllerBase
    {
        [HttpGet("{id}")]
        public int Get(string id)
        {
            int dönen = DateTime.Now.Second;

            return dönen;
        }

    }
}
