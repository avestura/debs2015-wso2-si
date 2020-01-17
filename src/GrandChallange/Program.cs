using CsvHelper;
using GrandChallange.Extensions;
using GrandChallange.Geography;
using GrandChallange.Models;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace GrandChallange
{
    public class Program
    {
        private string CSVPath = @"E:\Organize\sorted_data.csv";
        private readonly Uri FirstQueryUri = new Uri("http://localhost:8006/q1");
        private readonly Uri SecondQueryUri = new Uri("http://localhost:8007/q2");
        private readonly Uri ServiceQuery1Frequent = new Uri("https://localhost:5001/Query1Frequent");


        Timer FirstQueryResultTimer = new Timer(1000);
        Timer SecondueryResultTimer = new Timer(1000);

        public Program()
        {
            FirstQueryResultTimer.Elapsed += new ElapsedEventHandler(FirstQueryResultTimer_Elapsed);
            SecondueryResultTimer.Elapsed += new ElapsedEventHandler(SecondQueryResultTimer_Elapsed);
        }

        private void FirstQueryResultTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ = ShowFirstQueryResult();
        }

        private void SecondQueryResultTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ = ShowSecondQueryResult();
        }

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
            FirstQueryResultTimer.Start();
            FirstQueryInputModel newInput;

            using var reader = new StreamReader(CSVPath);
            using var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;

            int sendedEventCount = 1;
            int allReadedRowCount = 1;
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

                    SendEvent(JsonSerializer.Serialize(jsonModel), FirstQueryUri);

                    Console.Write("\r{0} event send from {1}", sendedEventCount, allReadedRowCount);
                    sendedEventCount++;
                }
                catch (Exception ex)
                {
                    continue;
                }
                finally
                {
                    allReadedRowCount++;
                }
            }
        }

        private void RunSecondQuery()
        {
            SecondueryResultTimer.Start();
            SecondQueryInputModel newInput;

            using var reader = new StreamReader(CSVPath);
            using var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;

            int sendedEventCount = 1;
            int allReadedRowCount = 1;
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

                    Console.Write("\r{0} event send from {1}", sendedEventCount, allReadedRowCount);
                    sendedEventCount++;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    allReadedRowCount++;
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

        private async Task ShowFirstQueryResult()
        {
            Query1Result preivous = new Query1Result();
            await Task.Run(() =>
            {
                try
                {
                    WebClient client = new WebClient();
                    string address = "http://localhost:5000/Query1Frequent?reqTimestamp=" +
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    string resultJson = client.DownloadString(address);
                    Query1Result result = JsonSerializer.Deserialize<Query1Result>(resultJson, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });


                    if (result.PickupDatetime != preivous.PickupDatetime && result.DropoffDatetime != preivous.DropoffDatetime)
                    {
                        Console.WriteLine("\n####################### Top 10 frequent routes #######################");
                        Console.Write(result.ToString());
                    }

                    preivous = result;
                }
                catch (Exception) { }
            });
        }

        private async Task ShowSecondQueryResult()
        {
            Query2Result preivous = new Query2Result();
            await Task.Run(() =>
            {
                try
                {
                    WebClient client = new WebClient();
                    string address = "http://localhost:5000/Query2Frequent?reqTimestamp=" +
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    string resultJson = client.DownloadString(address);
                    Query2Result result = JsonSerializer.Deserialize<Query2Result>(resultJson, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                    if (result.PickupDatetime != preivous.PickupDatetime && result.DropoffDatetime != preivous.DropoffDatetime)
                    {
                        Console.WriteLine("\n####################### Top 10 profitable area #######################");
                        Console.WriteLine(resultJson);
                    }

                    preivous = result;
                }
                catch (Exception) { }
            });
        }
    }
}
