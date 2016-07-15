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
                //Load the XML config file
                XmlDocument doc = new XmlDocument();
                doc.Load("Settings.XML");

                XmlNode uriNode = doc.SelectSingleNode("SETTINGS/API_URI"); //Find the setting we need

                if(uriNode != null) //Make sure the entry exists
                {
                    string uriString = uriNode.InnerText; //Get the URI to call

                    //Setup the http client
                    HttpClient hClient = new HttpClient(); 

                    hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //Setup JSON

                    using (HttpResponseMessage response = hClient.GetAsync(uriString).Result)  //Make the call to the API & makes sure connection is closed
                    {
                        //Check the response from the server
                        if (response.IsSuccessStatusCode)
                        {
                            var message = response.Content.ReadAsAsync(typeof(string)).Result; //Get the data send back from the API

                            if (message != null) //Make sure there is data there
                            {
                                if (!string.IsNullOrWhiteSpace((string)message)) //Make sure there is something to display
                                {
                                    Console.WriteLine("API Response: " + message); //Write the response to the console
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        }
                    }
                }
                else
                {
                    //Setting data is missing
                    Console.WriteLine("The Settings.XML configuration file is missing the API_URI setting.");
                }

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey(); //Wait for the user to hit a key so they can read the console
            }
            catch( Exception ex)
            {
                Console.WriteLine("An exception occured.  The details are: " + ex.ToString());
            }

        }
    }
}
