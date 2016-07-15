using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;

namespace HelloWorldConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Settings.XML");

                XmlNode uriNode = doc.SelectSingleNode("SETTINGS/API_URI");

                if(uriNode != null)
                {
                    string uriString = uriNode.InnerText;

                    HttpClient hClient = new HttpClient();

                    hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = hClient.GetAsync(uriString).Result;

                    if (response.IsSuccessStatusCode)
                    {

                        var message = response.Content.ReadAsAsync(typeof(string)).Result;

                        if(message != null)
                        {
                            if (!string.IsNullOrWhiteSpace((string)message))
                            {
                                Console.WriteLine("API Response: " + message);
                            }
                        }
                       
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }

                }

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            catch( Exception ex)
            {
                Console.WriteLine("An exception occured.  The details are: " + ex.ToString());
            }

        }
    }
}
