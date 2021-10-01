using SR2.DataLayer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SR2.PresentationConsole
{
    class Program
    {
        private static readonly HttpClient client = new();
        static void Main()
        {
            Console.WriteLine("Start.....");
            //Console.WriteLine("Hello World!");
            client.BaseAddress = new Uri("http://localhost:34978/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            //Get data from api
            GetBrandAsyn().Wait();
            Console.WriteLine("Finish");
            Console.ReadLine();

        }
        static async Task GetBrandAsyn()
        {
            HttpResponseMessage response = await client.GetAsync("api/brands");
            if (response.IsSuccessStatusCode)
            {
                var brands = await response.Content.ReadAsAsync<List<Brand>>();
                if (brands == null) Console.WriteLine("No record");
                foreach(var item in brands)
                {
                    Console.WriteLine("Brand name ={0}", item.BrandName);
                }
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}
