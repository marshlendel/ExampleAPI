using ExampleAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ExampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAppDataController : ControllerBase
    {
        [HttpGet("GetAllUserAppData")]
        public ActionResult<IEnumerable<object>> GetAllUserAppData()
        {
            List<UserAppXref> userAppXrefs;
            List<User> users;
            List<App> apps;

            using (var reader = new StreamReader("Data/userAppXref.json"))
            {
                string userAppXrefData = reader.ReadToEnd();
                var jsonObject = JObject.Parse(userAppXrefData);
                var userAppXrefArray = jsonObject["userAppXref"].ToString();
                userAppXrefs = JsonConvert.DeserializeObject<List<UserAppXref>>(userAppXrefArray);
            }

            using (var reader = new StreamReader("Data/userData.json"))
            {
                string userData = reader.ReadToEnd();
                var userList = JsonConvert.DeserializeObject<UserList>(userData);
                users = userList?.Users;
            }

            using (var reader = new StreamReader("Data/appData.json"))
            {
                string appData = reader.ReadToEnd();
                var appList = JsonConvert.DeserializeObject<AppList>(appData);
                apps = appList?.Apps;
            }

            if (userAppXrefs == null || users == null || apps == null)
            {
                return NotFound();
            }

            var userAppData = users.Select(user => new
            {
                User = user,
                Apps = userAppXrefs.Where(xref => xref.UserId == user.Id)
                                   .Join(apps, xref => xref.AppId, app => app.Id, (xref, app) => app)
                                   .ToList()
            }).ToList();

            return Ok(userAppData);
        }
    }
}
