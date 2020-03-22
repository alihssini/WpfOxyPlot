using System;
using System.Collections.Generic;
using System.Text;

namespace WpfChallenge.DataProvider
{
   public interface IPointsFileReader
    {
        IEnumerable<Point> GetAllPoints(string filePath);
    }
}
