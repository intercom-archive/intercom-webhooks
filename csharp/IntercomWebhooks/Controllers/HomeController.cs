using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNet.Http;
using System.IO;
using System.Security.Cryptography;

namespace IntercomWebhooks.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
      [HttpPost]
      public void Post()
      {
        var payload_body = new StreamReader(Request.Body).ReadToEnd();
        Console.WriteLine("===============================================================");
        Console.WriteLine(payload_body);
        Console.WriteLine("===============================================================");
        verify_signature (payload_body);
        var output = JsonConvert.DeserializeObject<dynamic>(payload_body);
        Console.WriteLine ("Topic Recieved: " + output.topic);
      }
      private void verify_signature(string payload_body)
      {
        var secret = "secret";
        String expected = Request.Headers["X-Hub-Signature"];
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
