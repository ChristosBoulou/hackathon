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
