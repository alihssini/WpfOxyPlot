using OxyPlot;
using WpfChallenge.Model;
using System.Collections.Generic;

namespace WpfChallenge.DataProvider
{
    public interface IFitter
    {
        FitData CalcFit(List<DataPoint> points);
    }
}
