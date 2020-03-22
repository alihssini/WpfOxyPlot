using MathNet.Numerics;
using OxyPlot;
using WpfChallenge.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfChallenge.DataProvider
{
    public class PowerFitter : IFitter
    {
        public FitData CalcFit(List<DataPoint> points)
        {
            FitData result = new FitData("Power function");
            var xdata = points.Select(x => x.X).ToArray();
            var ydata = points.Select(x => x.Y).ToArray();
            Tuple<double, double> p = Fit.Power(xdata, ydata);
            result.A = p.Item1;
            result.B = p.Item2;

            foreach (var point in points)
                result.Points.Add(new DataPoint(point.X, result.A * Math.Exp(result.B * point.X)));
            
            return result;
        }
    }
}
