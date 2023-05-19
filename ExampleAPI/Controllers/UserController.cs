using ExampleAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("GetAllUsers")]
        public UserList GetAllUsers()
        {
            using (StreamReader reader = new StreamReader("Data/userData.json"))
            {
                string jsonData = reader.ReadToEnd();
                UserList users = JsonConvert.DeserializeObject<UserList>(jsonData);
                return users;
            }
        }
    }
}