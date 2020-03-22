using OxyPlot;
using System.Collections.Generic;

namespace WpfChallenge.Model
{
    public class FitData
    {
        public FitData(string title)
        {
            Title = title;
            Points = new List<DataPoint>();
        }
        public double A { get; set; }
        public double B { get; set; }
        public double R { get; set; }
        public string Title { get; set; }
        public List<DataPoint> Points { get; set; }
    }
}
