using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class DemoController : Controller
    {
        public string HelloWorld()
        {
            return "Se07301 - ASP.Net MVC";
            // https://localhost:[port]/Demo/HelloWorld
        }
        public string Goodbye()
        {
            return "See you again!";
            // https://localhost:[port]/Demo/Goodbye
        }
        public string Post(int id, string name)
        {
            return $"Ma bai viet : {id} - Ten bai viet : {name}";
            /// https://localhost:[port]/Demo/Post?id=10&name=Toan
        }
    }
}
