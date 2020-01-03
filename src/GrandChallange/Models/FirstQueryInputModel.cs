namespace GrandChallange.Models
{
    public class FirstQueryInputModel
    {
        public long PickupTime { get; set; }
        public long DropoffTime { get; set; }
        public string PickCell { get; set; }
        public string DropCell { get; set; }
        public long EventTimestamp { get; set; }
        public string PickupTimeOrig { get; set; }
        public string DropoffTimeOrig { get; set; }
    }
}
