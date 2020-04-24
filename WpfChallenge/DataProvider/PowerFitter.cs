﻿using MathNet.Numerics;
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

            result.Points = DataPoints(points).ToList();

            return result;

            //local function for return list of regresion Points[need to C# 8]
            IEnumerable<DataPoint> DataPoints(List<DataPoint> points)
            {
                foreach (var point in points)
                    yield return new DataPoint(point.X, result.A * Math.Exp(result.B * point.X));
            }
        }
    }
}
