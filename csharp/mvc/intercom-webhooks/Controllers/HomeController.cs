using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;

namespace intercomwebhooks.Controllers
{
    public class HomeController : Controller
    {
		[HttpPost]
		public ActionResult Index()
		{
			StreamReader stream = new StreamReader(Request.InputStream);
			string payload_body = stream.ReadToEnd();

			Console.WriteLine("===============================================================");
			Console.WriteLine(payload_body);
			Console.WriteLine("===============================================================");
			verify_signature (payload_body);
			var output = JsonConvert.DeserializeObject<dynamic>(payload_body);
			Console.WriteLine ("Topic Recieved: " + output.topic);
            return View ();
        }
		private void verify_signature(string payload_body){
			var secret = "secret";

			String expected = Request.Headers.Get("X-Hub-Signature");
			if(string.IsNullOrWhiteSpace(expected)){
				Console.WriteLine("Not signed. Not calculating");
			}
			else{
				var signature = "sha1=" + CreateToken(payload_body, secret);
				Console.WriteLine("Expected  : " + expected);
				Console.WriteLine("Calculated: " + signature);
				Console.WriteLine("Match?    : " + signature.Equals(expected,StringComparison.OrdinalIgnoreCase));
			}
		}

		private string CreateToken(string message, string secret)
		{
			secret = secret ?? "";
			var encoding = new System.Text.ASCIIEncoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha1 = new HMACSHA1(keyByte))
			{
				byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

				var sb = new System.Text.StringBuilder();
				for (var i = 0; i <= hashmessage.Length - 1; i++)
				{
					sb.Append(hashmessage[i].ToString("X2"));
				}
				return sb.ToString();
			}
		}

    }
}