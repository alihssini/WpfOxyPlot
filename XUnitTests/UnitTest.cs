using Moq;
using WpfChallenge;
using WpfChallenge.DataProvider;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTests
{
    public class UnitTest
    {
        [Theory]
        [InlineData(1.1, 1.4000000001489119, 4.4607199641933963, 18.089111453594121, 1, 1.1, 2, 2.902917404)]
        public void PowerFitter(double expectedA, double expectedB, double expected1, double expected2, double x1, double y1, double x2, double y2)
        {
            var fit = new PowerFitter();
            var list = new System.Collections.Generic.List<OxyPlot.DataPoint>();
            list.Add(new OxyPlot.DataPoint(x1, y1));
            list.Add(new OxyPlot.DataPoint(x2, y2));
            var result = fit.CalcFit(list);
            Assert.Equal(expectedA, result.A);
            Assert.Equal(expectedB, result.B);
            Assert.Equal(expected1, result.Points[0].Y);
            Assert.Equal(expected2, result.Points[1].Y);
        }
        [Theory]
        [InlineData(0.10000000014315484, 0.20000000007477742, 0.12214027600000005, 0.14918246999999993, 1, 0.122140276, 2, 0.14918247)]
        public void ExponentialFitter(double expectedA, double expectedR, double expected1, double expected2, double x1, double y1, double x2, double y2)
        {
            var fit = new ExponentialFitter();
            var list = new System.Collections.Generic.List<OxyPlot.DataPoint>();
            list.Add(new OxyPlot.DataPoint(x1, y1));
            list.Add(new OxyPlot.DataPoint(x2, y2));
            var result = fit.CalcFit(list);
            Assert.Equal(expectedA, result.A);
            Assert.Equal(expectedR, result.R);
            Assert.Equal(expected1, result.Points[0].Y);
            Assert.Equal(expected2, result.Points[1].Y);
        }
        [Theory]
        [InlineData(-1, 2, 1, -1, 1, 1, 3, 5)]
        public void LinearFitter(double expectedA, double expectedB, double expected1, double expected2, double x1, double y1, double x2, double y2)
        {
            var fit = new LinearFitter();
            var list = new System.Collections.Generic.List<OxyPlot.DataPoint>();
            list.Add(new OxyPlot.DataPoint(x1, y1));
            list.Add(new OxyPlot.DataPoint(x2, y2));
            var result = fit.CalcFit(list);
            Assert.Equal(expectedA, result.A);
            Assert.Equal(expectedB, result.B);
            Assert.Equal(expected1, result.Points[0].Y);
            Assert.Equal(expected2, result.Points[1].Y);
        }
        [Fact]
        public void PointsFileReader()
        {
            var reader = new PointsFileReader();
            var data= reader.GetAllPoints(@"Data\TestData.txt");
            Assert.Equal(3, data.Count());

            Assert.Equal(2, data.ElementAt(0).X);
            Assert.Equal(3, data.ElementAt(0).Y);

            Assert.Equal(5, data.ElementAt(1).X);
            Assert.Equal(9, data.ElementAt(1).Y);

            Assert.Equal(2.5, data.ElementAt(2).X);
            Assert.Equal(3.2, data.ElementAt(2).Y);
        }

        [Fact]
        public void LinearRegression()
        {
            var moq = new Mock<IPointsFileReader>();
                moq.Setup(m => m.GetAllPoints("")).Returns(
                new List<Point>() { 
                    new Point { X = 1, Y = 1 },
                    new Point { X =3, Y = 5 } 
                });
           
            var viewModel = new MainViewModel(moq.Object);
            
            viewModel.SelectedMode = "Linear";
            Assert.Equal(2, viewModel.Points.Count);
            Assert.Equal(2, viewModel.Regression.Count);

            Assert.Equal("Linear", viewModel.Title);

            Assert.Equal(3, viewModel.Regression[1].X);
            Assert.Equal(-1, viewModel.Regression[1].Y);
        }
        [Fact]
        public void ExponentialRegression()
        {
            var moq = new Mock<IPointsFileReader>();
            moq.Setup(m => m.GetAllPoints("")).Returns(
            new List<Point>() {
                    new Point { X = 1, Y = 0.122140276 },
                    new Point { X =2, Y = 0.14918247 },
                        new Point { X =3, Y = 0.18221188 },
            });

            var viewModel = new MainViewModel(moq.Object);

            viewModel.SelectedMode = "Exponential";
            Assert.Equal(3, viewModel.Points.Count);
            Assert.Equal(3, viewModel.Regression.Count);

            Assert.Equal("Exponential", viewModel.Title);

            Assert.Equal(0.12214027603807083, viewModel.Regression[0].Y);
            Assert.Equal(0.14918246990699954, viewModel.Regression[1].Y);
            Assert.Equal(0.182211880056795261, viewModel.Regression[2].Y);
        }
        [Fact]
        public void PowerRegression()
        {
            var moq = new Mock<IPointsFileReader>();
            moq.Setup(m => m.GetAllPoints("")).Returns(
            new List<Point>() {
                    new Point { X = 1, Y = 1.1 },
                    new Point { X =2, Y = 2.902917404 }
            });

            var viewModel = new MainViewModel(moq.Object);

            viewModel.SelectedMode = "Power function";
            Assert.Equal(2, viewModel.Points.Count);
            Assert.Equal(2, viewModel.Regression.Count);

            Assert.Equal("Power function", viewModel.Title);
            
            Assert.Equal(4.4607199641933963, viewModel.Regression[0].Y);
            Assert.Equal( 18.089111453594121, viewModel.Regression[1].Y);
        }
    }
}
