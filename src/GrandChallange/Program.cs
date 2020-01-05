using CsvHelper;
using GrandChallange.Extensions;
using GrandChallange.Geography;
using GrandChallange.Models;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace GrandChallange
{
    class Program
    {
        private string CSVPath = @"D:\University\Distributed Systems\dd.csv";
        private Uri URI = new Uri("http://172.17.8.167:8006/q1");

        static void Main(string[] args)
        {
            Program program = new Program();

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++ \n" +
                "| Welcom to Grand Challenge 2015 solution | \n" +
                "+++++++++++++++++++++++++++++++++++++++++++");

            bool correctInput = false;

            while (!correctInput)
            {
                Console.WriteLine("\nPlease enter query number for run");
                Console.WriteLine("\n1.first query \n2.second query");
                Console.Write("\ninput your number: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        correctInput = true;
                        program.RunFirstQuery();
                        break;
                    case "2":
                        correctInput = true;
                        program.RunSecondQuery();
                        break;
                    default:
                        Console.WriteLine("Number was wrong!");
                        break;
                }

                Console.WriteLine("File fully read.\nResult:\n");
                program.ShowResult();
            }
        }

        private void RunFirstQuery()
        {
            TimeStamp timeStamp = new TimeStamp();
            FirstQueryInputModel newInput;

            using (var reader = new StreamReader(CSVPath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;

                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<DataModel>();

                        var pickUpLocation = new TaxiLocation
                            (
                                new Coordinates(double.Parse(record.PickupLongitude), double.Parse(record.PickupLatitude)),
                                QueryRespect.RespectQuery1
                            );

                        var dropOffLocation = new TaxiLocation
                            (
                                new Coordinates(double.Parse(record.DropoffLongitude), double.Parse(record.DropoffLatitude)),
                                QueryRespect.RespectQuery1
                            );

                        newInput = new FirstQueryInputModel
                        {
                            PickupTime = timeStamp.GetUnixTime(record.PickupDatetime),
                            DropoffTime = timeStamp.GetUnixTime(record.DropoffDatetime),
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
                        Console.WriteLine("Error: " + ex.Message);
                        continue;
                    }
                }
            }
        }

        private void RunSecondQuery()
        {
            TimeStamp timeStamp = new TimeStamp();
            SecondQueryInputModel newInput;

            using (var reader = new StreamReader(CSVPath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = false;

                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<DataModel>();

                        var pickUpLocation = new TaxiLocation
                            (
                                new Coordinates(double.Parse(record.PickupLongitude), double.Parse(record.PickupLatitude)),
                                QueryRespect.RespectQuery2
                            );

                        var dropOffLocation = new TaxiLocation
                            (
                                new Coordinates(double.Parse(record.DropoffLongitude), double.Parse(record.DropoffLatitude)),
                                QueryRespect.RespectQuery2
                            );

                        newInput = new SecondQueryInputModel
                        {
                            Medallion = record.Medallion,
                            PickupTime = timeStamp.GetUnixTime(record.PickupDatetime),
                            DropoffTime = timeStamp.GetUnixTime(record.DropoffDatetime),
                            PickCell = pickUpLocation.CellId,
                            DropCell = dropOffLocation.CellId,
                            EventTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            PickupTimeOrig = record.PickupDatetime,
                            DropoffTimeOrig = record.DropoffDatetime,
                            FareAmount = float.Parse(record.FareAmount),
                            TipAmount = float.Parse(record.TipAmount)
                        };

                        var jsonModel = new
                        {
                            @event = newInput
                        };

                        SendEventAsync(JsonSerializer.Serialize(jsonModel));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        continue;
                    }
                }
            }
        }

        private void SendEventAsync(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadStringAsync(URI, json);
            }
        }

        private void ShowResult()
        {
            WebClient client = new WebClient();
            string address = "http://localhost:8080/result.txt";
            // Save the file to desktop for debugging
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = desktop + "\\result.txt";
            client.DownloadFile(address, fileName);           
        }
    }
}
