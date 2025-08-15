using Microsoft.AspNetCore.Mvc;

namespace DentalHealthFollow_Up.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipsController : ControllerBase
    {
        private static readonly string[] Tips = new[]
        {
            "Diş ipini her akşam kullan.",
            "Şekerli içeceklerden sonra su iç.",
            "Florürlü diş macunu tercih et.",
            "Yatmadan önce atıştırma.",
            "Diş fırçanı 3 ayda bir değiştir."
        };
        // GET api/tips
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAll() => Ok(Tips);

        // GET api/tips/random
        [HttpGet("random")]
        public ActionResult<string> GetRandom()
        {
            var tip = Tips[Random.Shared.Next(Tips.Length)];
            return Ok(tip);
        }
    }
}
