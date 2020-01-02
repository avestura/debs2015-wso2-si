namespace GrandChallange.Geography
{
    public struct Coordinates
    {
        /// <summary>
        /// Determines West-East coordination
        /// </summary>
        public readonly double Longitude { get; }

        /// <summary>
        /// Determines North-South coordination
        /// </summary>
        public readonly double Latitude { get; }

        public Coordinates(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public static implicit operator Coordinates((double, double) coordinates)
            => new Coordinates(coordinates.Item1, coordinates.Item2);
    }
}
