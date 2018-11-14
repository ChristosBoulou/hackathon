using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace OcrFunctions.Services
{
    public static class BoundingBoxHelper
    {
        public static Coordinates GetMaxXandY(List<Line> lines)
        {
            List<Coordinates> allCoordinates = new List<Coordinates>();
            foreach(var line in lines)
            {
                allCoordinates.Add(new Coordinates(line.BoundingBox[0], line.BoundingBox[1]));
                allCoordinates.Add(new Coordinates(line.BoundingBox[2], line.BoundingBox[3]));
                allCoordinates.Add(new Coordinates(line.BoundingBox[4], line.BoundingBox[5]));
                allCoordinates.Add(new Coordinates(line.BoundingBox[6], line.BoundingBox[7]));
            }

            return new Coordinates(allCoordinates.Select(co => co.X).Max(), allCoordinates.Select(co => co.Y).Max());
        }

        public static Tuple<bool, bool> IsWithinThisPercentage(Line line, Coordinates maxCoordinates, int maxXPercentage, int maxYPercentage)
        {
            var topRightCoordinates = new Coordinates(line.BoundingBox[3], line.BoundingBox[4]);
            var bottomRightCoordinates = new Coordinates(line.BoundingBox[5], line.BoundingBox[6]);

            bool withinX = false;
            if (topRightCoordinates.X/ maxCoordinates.X <= (maxXPercentage/ 100))
            {
                withinX = true;
            }

            bool withinY = false;
            if (bottomRightCoordinates.Y / maxCoordinates.Y <= (maxYPercentage / 100))
            {
                withinY = true;
            }

            return new Tuple<bool, bool>(withinX, withinY);
        }
    }



    public class Coordinates 
    {
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

    }
}
