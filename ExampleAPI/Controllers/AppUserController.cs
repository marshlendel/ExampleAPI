using ExampleAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

namespace ExampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppUserController : ControllerBase
    {
        [HttpGet("GetAppsByUserId/{userId}")]
        public ActionResult<IEnumerable<App>> GetAppsByUserId(int userId)
        {
            List<UserAppXref> userAppXrefs;
            List<App> apps;

            using (var reader = new StreamReader("Data/userAppXref.json"))
            {
                string userAppXrefData = reader.ReadToEnd();
                var jsonObject = JObject.Parse(userAppXrefData);
                var userAppXrefArray = jsonObject["userAppXref"].ToString();
                userAppXrefs = JsonConvert.DeserializeObject<List<UserAppXref>>(userAppXrefArray);
            }

            using (var reader = new StreamReader("Data/appData.json"))
            {
                string appData = reader.ReadToEnd();
                var appList = JsonConvert.DeserializeObject<AppList>(appData);
                apps = appList?.Apps;
            }

            if (userAppXrefs == null || apps == null)
            {
                return NotFound();
            }

            var userApps = userAppXrefs.Where(x => x.UserId == userId)
                                       .Join(apps, xref => xref.AppId, app => app.Id, (xref, app) => app)
                                       .ToList();

            if (!userApps.Any())
            {
                return NotFound();
            }

            return Ok(userApps);
        }

        [HttpGet("GetUsersByAppId/{appId}")]
        public ActionResult<IEnumerable<User>> GetUsersByAppId(int appId)
        {
            List<UserAppXref> userAppXrefs;
            List<User> users;

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

            if (userAppXrefs == null || users == null)
            {
                return NotFound();
            }

            var appUsers = userAppXrefs.Where(xref => xref.AppId == appId)
                                       .Join(users, xref => xref.UserId, user => user.Id, (xref, user) => user)
                                       .ToList();

            if (!appUsers.Any())
            {
                return NotFound();
            }

            return Ok(appUsers);
        }

    }
}
