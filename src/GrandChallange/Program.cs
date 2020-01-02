using CsvHelper;
using GrandChallange.Extensions;
using GrandChallange.Geography;
using GrandChallange.Models;
using System;
using System.IO;
using System.Linq;

namespace GrandChallange
{
    class Program
    {
        private string csvPath = @"D:\University\DSL\dd.csv";

        static void Main(string[] args)
        {
            Program program = new Program();
            program.ReadInput();
        }

        public void ReadInput() 
        {
            TimeStamp timeStamp = new TimeStamp();
            FirstQueryInputModel newInput;

            using (var reader = new StreamReader(csvPath))
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
                            PickUpTime = timeStamp.GetUnixTime(record.PickupDatetime),
                            DropOffTime = timeStamp.GetUnixTime(record.DropoffDatetime),
                            PickUpTimeOrig = record.PickupDatetime,
                            DropOffTimeOrig = record.DropoffDatetime,
                            PickCellId = pickUpLocation.CellId,
                            DropCellId = dropOffLocation.CellId,
                            EventTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                        };
                    }
                    catch (Exception ex)
                    {
                        string dd = ex.Message;
                        continue;
                    }
                }
            }
        }
    }
}
