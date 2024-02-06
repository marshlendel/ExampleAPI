using ExampleAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        [HttpGet("GetAllApps")]
        public ActionResult<AppList> GetAllApps()
        {
            using (StreamReader reader = new StreamReader("Data/appData.json"))
            {
                string jsonData = reader.ReadToEnd();
                AppList apps = JsonConvert.DeserializeObject<AppList>(jsonData);
                if (apps == null)
                {
                    return NotFound();
                }
                return Ok(apps);
            }
        }
    }
}
