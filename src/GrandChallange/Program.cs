using CsvHelper;
using GrandChallange.Extensions;
using GrandChallange.Geography;
using GrandChallange.Models;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace GrandChallange
{
    public class Program
    {
        private string CSVPath = @"E:\Organize\small_data\sorted_data.csv";
        private readonly Uri FirstQueryUri = new Uri("http://localhost:8006/q1");
        private readonly Uri SecondQueryUri = new Uri("http://localhost:8007/q2");
        private readonly Uri ServiceQuery1Frequent = new Uri("https://localhost:5001/Query1Frequent");


        private static DateTime LastDateTime { get; set; } = DateTime.Now;

        private static long LastDropoff { get; set; } = 0;

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
            }
        }

        private void RunFirstQuery()
        {
            Console.WriteLine("Sending events...");
            FirstQueryInputModel newInput;

            using var reader = new StreamReader(CSVPath);
            using var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;

            int sentEventsCount = 0;
            int usentEventsCount = 0;
            int logFilterCount = 0;
            int allCount = 0;
            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<DataModel>();

                    var dropTime = record.DropoffDatetime.GetUnixTime();
                    LastDropoff = Math.Max(LastDropoff, dropTime);
                    var _30minAgo = LastDropoff - (30 * 60 * 1000);

                    if (_30minAgo > dropTime) continue;

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

                    SendEvent(JsonSerializer.Serialize(jsonModel), FirstQueryUri);
                    sentEventsCount++;
                }
                catch (Exception)
                {
                    usentEventsCount++;
                }
                finally
                {
                    logFilterCount++;
                    allCount++;
                    if (logFilterCount > 1000)
                    {
                        var now = DateTime.Now;
                        Console.Write($"All: {allCount}    Sent events: {sentEventsCount}      Unsent events: {usentEventsCount}, {now.ToLongTimeString()}, Took ");
                        var d = now - LastDateTime;
                        Console.ForegroundColor = (d > TimeSpan.FromSeconds(10)) ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine(d.ToString());
                        Console.ResetColor();
                        LastDateTime = now;
                        logFilterCount = 0;
                    }
                }
            }
        }

        private void RunSecondQuery()
        {
            SecondQueryInputModel newInput;

            using var reader = new StreamReader(CSVPath);
            using var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;

            int sentEventsCount = 0;
            int usentEventsCount = 0;
            int logFilterCount = 0;
            int allCount = 0;
            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<DataModel>();

                    var dropTime = record.DropoffDatetime.GetUnixTime();
                    LastDropoff = Math.Max(LastDropoff, dropTime);
                    var _30minAgo = LastDropoff - (30 * 60 * 1000);

                    if (_30minAgo > dropTime) continue;

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
                        PickupTime = record.PickupDatetime.GetUnixTime(),
                        DropoffTime = record.DropoffDatetime.GetUnixTime(),
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

                    SendEvent(JsonSerializer.Serialize(jsonModel), SecondQueryUri);
                    sentEventsCount++;
                }
                catch (Exception)
                {
                    usentEventsCount++;
                }
                finally
                {
                    logFilterCount++;
                    allCount++;
                    if (logFilterCount > 1000)
                    {
                        var now = DateTime.Now;
                        Console.Write($"All: {allCount}    Sent events: {sentEventsCount}      Unsent events: {usentEventsCount}, {now.ToLongTimeString()}, Took ");
                        var d = now - LastDateTime;
                        Console.ForegroundColor = (d > TimeSpan.FromSeconds(10)) ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine(d.ToString());
                        Console.ResetColor();
                        LastDateTime = now;
                        logFilterCount = 0;
                    }
                }
            }
        }

        private void SendEvent(string json, Uri uri)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            using WebClient webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringAsync(uri, json);
        }
    }
}
