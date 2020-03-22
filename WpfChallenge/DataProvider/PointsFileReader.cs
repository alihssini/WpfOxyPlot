using System;
using System.Collections.Generic;
using System.IO;

namespace WpfChallenge.DataProvider
{
    public class PointsFileReader : IPointsFileReader
    {
        public IEnumerable<Point> GetAllPoints(string filePath)
        {
           
            List<Point> result = new List<Point>();
            if (String.IsNullOrEmpty(filePath)) return result;

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var parts = line.Split(',');
                        if (parts.Length != 2) continue;
                        result.Add(new Point { X = double.Parse(parts[0]), Y = double.Parse(parts[1]) });
                    }
                    catch { }
               
                }
            }
            return result;
        }
    }
}
