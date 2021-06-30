using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blogg.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReverseController : ControllerBase
    {
        public static int ConcurrentCalls = 0;

        [HttpGet]
        public async Task<string> Get(string text)
        {
            if (ConcurrentCalls > 100)
            {
                throw new HttpRequestException("Too many calls", null, HttpStatusCode.TooManyRequests);
            }

            Interlocked.Increment(ref ConcurrentCalls);

            var rnd = new Random();

            await Task.Delay(rnd.Next(50, 5000));

            var response = new string(text.ToCharArray().Reverse().ToArray()) + " " + ConcurrentCalls;

            Interlocked.Decrement(ref ConcurrentCalls);

            return response;
        }
    }
}
