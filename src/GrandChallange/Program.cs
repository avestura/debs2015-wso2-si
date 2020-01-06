using CsvHelper;
using GrandChallange.Extensions;
using GrandChallange.Geography;
using GrandChallange.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrandChallange
{
    class Program
    {
        private string csvPath = @"D:\University\DSL\dd.csv";
        private string URI = "http://172.17.8.167:8006/q1";

        static void Main(string[] args)
        {
            Program program = new Program();
            program.ReadInput();

            Console.ReadKey();
        }

        public void ReadInput()
        {
            FirstQueryInputModel newInput;

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;

            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<DataModel>();

                    var pickUpLocation = new TaxiLocation
                        (
                            (double.Parse(record.PickupLongitude), double.Parse(record.PickupLatitude)),
                            QueryRespect.RespectQuery1
                        );

                    var dropOffLocation = new TaxiLocation
                        (
                            new Coordinates(double.Parse(record.DropoffLongitude), double.Parse(record.DropoffLatitude)),
                            QueryRespect.RespectQuery1
                        );

                    newInput = new FirstQueryInputModel
                    {
                        PickupTime = record.PickupDatetime.GetUnixTime(),
                        DropoffTime = record.DropoffDatetime.GetUnixTime(),
                        PickupTimeOrig = record.PickupDatetime,
                        DropoffTimeOrig = record.DropoffDatetime,
                        PickCell = pickUpLocation.CellId,
                        DropCell = dropOffLocation.CellId,
                        EventTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    };

                    var jsonModel = new
                    {
                        @event = newInput
                    };

                    SendEventAsync(JsonSerializer.Serialize(jsonModel));
                }
                catch (Exception ex)
                {
                    string dd = ex.Message;
                    continue;
                }
            }
        }

        private async Task SendEventAsync(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            Console.WriteLine("Send new event to server");
            Console.WriteLine("Waiting...");

            using (WebClient webClient = new WebClient())
            {
                /*
                HttpClient client = new HttpClient();

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.PostAsync(URI, content).Result;

                Console.WriteLine(result.ToString());
                Console.WriteLine("Done :)");
                */



                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                //webClient.UseDefaultCredentials = true;
                //webClient.Credentials = new NetworkCredential("admin", "admin");
                string HtmlResult = webClient.UploadString(URI, json);

                Console.WriteLine(HtmlResult);
                Console.WriteLine("Done :)");

            }

            //await Task.Run(() =>
            //{

            //    try
            //    {
            //        using (WebClient wc = new WebClient())
            //        {
            //            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            //            string HtmlResult = wc.UploadString(URI, json);
            //        }
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }

            //});


        }
    }
}
