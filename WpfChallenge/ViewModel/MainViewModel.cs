using OxyPlot;
using WpfChallenge.DataProvider;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;
using WpfChallenge.Utilities;
using System.ComponentModel;

namespace WpfChallenge
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Vars
        IPointsFileReader _fileReader;
        IFitter _fittedCurve;
        string _title = "Chalenge", _subTitle = "";
        string _selectedMode = "Linear";
        #endregion
        #region Props
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }
        public string SubTitle { get { return _subTitle; } set { _subTitle = value; OnPropertyChanged("SubTitle"); } }
        public PlotModel Model { get; private set; }
       

       
    
        public string SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                _selectedMode = value;

                if (_selectedMode.Contains("Power function"))
                    _fittedCurve = new PowerFitter();
                else if (_selectedMode.Contains("Exponential"))
                    _fittedCurve = new ExponentialFitter();
                else
                    _fittedCurve = new LinearFitter();

                LoadFittedCurve();
            }
        }
        public ObservableCollection<DataPoint> Points { get; private set; }
        public ObservableCollection<DataPoint> Regression { get; private set; }

        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Commands
        public RelayCommand OpenFile { get; private set; }
        #endregion
        #region Ctor
        public MainViewModel(IPointsFileReader fileReader)
        {
            OpenFile = new RelayCommand((o) =>
            {
                Load();
            });
            _fileReader = fileReader;
            Points = new ObservableCollection<DataPoint>();
            Regression = new ObservableCollection<DataPoint>();
            Model = new PlotModel();
            LoadPoints();
        }
        #endregion
        #region Funcs
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt|csv files (*.csv)|*.csv";
          
            var show = ofd.ShowDialog();
            if (show.HasValue && show.Value)
            {
                var filePath = ofd.FileName;
                LoadPoints(filePath);
            }

            LoadFittedCurve();
        }
        private void LoadPoints(string filePath = "")
        {
            Points.Clear();
            foreach (var point in _fileReader.GetAllPoints(filePath))
            {
                Points.Add(new DataPoint(
                  point.X, point.Y));
            }
        }
        private void LoadFittedCurve()
        {
            if (Points.Count > 0 && _fittedCurve != null)
            {
                Regression.Clear();
                var result = _fittedCurve.CalcFit(Points.ToList());
                result.Points.ForEach(x => Regression.Add(x));

                Title = result.Title;
                SubTitle = $"A : {result.A} , B : {result.B} , R : {result.R}";
            }
        }
        #endregion
    }
}
