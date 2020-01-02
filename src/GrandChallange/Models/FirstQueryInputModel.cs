namespace GrandChallange.Models
{
    public class FirstQueryInputModel
    {
        public long PickUpTime { get; set; }
        public long DropOffTime { get; set; }
        public string PickCellId { get; set; }
        public string DropCellId { get; set; }
        public long EventTimeStamp { get; set; }
        public string PickUpTimeOrig { get; set; }
        public string DropOffTimeOrig { get; set; }
    }
}
