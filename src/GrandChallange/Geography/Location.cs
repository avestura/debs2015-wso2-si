using System;
using System.Collections.Generic;
using System.Text;

namespace GrandChallange.Geography
{


    /// <summary>
    /// This class maps a coordination like (41.47493, -74.9135) to its corresponding map cell like (1, 3)
    /// </summary>
    public class TaxiLocation
    {
        public const double EastDistancePer500Meters = 0.005986;
        public const double SouthDistancePer500Meters = 0.004491556;

        public static Coordinates CenterOfFirstCell => (41.474937, -74.913585);

        public static Coordinates ZeroZeroCoordinates => (41.471944, -74.915830778);

        public QueryRespect QueryRespect { get; }

        public int MinCellIndex => 1;

        public int MaxCellIndex => (QueryRespect == QueryRespect.RespectQuery1) ? 300 : 600;

        public Coordinates Coordinates { get; }

        public int X
        {
            get
            {
                var dx = (QueryRespect == QueryRespect.RespectQuery1) ?
                    EastDistancePer500Meters :
                    EastDistancePer500Meters / 2;

                int index = (int)((Coordinates.Longitude - ZeroZeroCoordinates.Longitude) / dx);
                if (index < MinCellIndex || index > MaxCellIndex)
                    throw new Exception("Index is out of the bounds of map.");
                return index;
            }
        }

        public int Y
        {
            get
            {
                var dy = (QueryRespect == QueryRespect.RespectQuery1) ?
                                    SouthDistancePer500Meters :
                                    SouthDistancePer500Meters / 2;

                int index = (int)((Coordinates.Latitude - ZeroZeroCoordinates.Latitude) / dy);
                if (index < MinCellIndex || index > MaxCellIndex)
                    throw new Exception("Index is out of the bounds of map.");
                return index;
            }
        }

        public TaxiLocation(Coordinates coordinates, QueryRespect queryRespect)
        {
            QueryRespect = queryRespect;
            Coordinates = coordinates;
        }
    }

    public enum QueryRespect
    {
        RespectQuery1, RespectQuery2
    }
}
