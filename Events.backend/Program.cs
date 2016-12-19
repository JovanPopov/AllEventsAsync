using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;



namespace Events.backend
{
    class Program
    {
        private static List<Datum> events1 = new List<Datum>();
        private static int calls = 0;
        private static int ci = 0;
        private static int nextStep=100;
        private static int increment = 1;
        private static bool helper = false;
        private static int numCalls = 0;
        private static Stopwatch stopWatch;
        static void Main()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("Hit ENTER to exit...");
            callMore();


            Console.ReadLine();
        }

        public static void callMore()
        {

            Console.WriteLine(nextStep + "more");
            numCalls = 0;
            
            for (int i = ci; i < ci+nextStep ; i++)
            {
                numCalls++;
                Console.WriteLine("call started " + i);
                //Task.Run(() => MakeRequest(i, Completed));
                MakeRequest(i, Completed);
            }
            ci = ci + nextStep;
            nextStep = nextStep * increment;
           // ci = ci * ci;
            //ci = ci + nextStep;
        }

        private static void Completed(RootObject file)
        {
            lock (events1)
            {
                if (file.data != null)
                {
                    if (file.data.Count != 0)
                    {
                        events1.AddRange(file.data);

                        if (file.data.Count < 100)
                        {
                            helper = true;
                        }
                    }
                }
            //Console.WriteLine("Call...");
            if (calls == numCalls)
                results();
            //this method will be called whenever a file is processed
            }
        }

        private static void results()
        {
            if (!helper)
            {
                calls = 0;
               // Console.WriteLine(nextStep*100 + " more");
                callMore();


            }
            else
            {

                //foreach (Datum d in events1)
                //{
                //    Console.WriteLine(d.eventname);
                //}

                Console.WriteLine(events1.Count);
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
            }
        }

        static async void MakeRequest(int page, Action<RootObject> callback)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "5b298f3bad504791a7052c6d6257f97b");

            // Request parameters

            queryString["city"] = "New York";
            queryString["state"] = "NY";
            queryString["country"] = "United States";


            //queryString["city"] = "Novi Sad";
            //queryString["state"] = "VO";
            //queryString["country"] = "Yugoslavia";


            queryString["page"] = page.ToString();
            var uri = "https://api.allevents.in/events/list/?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }

            string Text = await response.Content.ReadAsStringAsync();
            RootObject root = new JavaScriptSerializer().Deserialize<RootObject>(Text);


          

            calls++;
            callback(root);
        }
    }
}
