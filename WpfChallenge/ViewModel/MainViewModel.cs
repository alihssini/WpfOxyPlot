using OxyPlot;
using WpfChallenge.DataProvider;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;
using WpfChallenge.Utilities;
using System.ComponentModel;
using System;
using System.Windows;
using System.Collections.Generic;
using WpfChallenge.Model;
using WpfChallenge.Enums;

namespace WpfChallenge
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Vars
        IPointsFileReader _fileReader;
        IFitter _fittedCurve;
        string _title = "Chalenge", _subTitle = "";
        FitMode _selectedMode;
        #endregion
        #region Props
        public List<FitMode> Modes { get; } = new List<FitMode>
        {
             new FitMode{ModeTitle= "Linear",ModeValue=Mode.Linear },
             new FitMode{ModeTitle= "Exponential",ModeValue=Mode.Exponential },
             new FitMode{ModeTitle= "Power function",ModeValue=Mode.PowerFunction }
        };
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }
        public string SubTitle { get { return _subTitle; } set { _subTitle = value; OnPropertyChanged("SubTitle"); } }
        public PlotModel Model { get; private set; }




        public FitMode SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                _selectedMode = value;
                //Switch expression to handle fitter model[need to C# 8]
                _fittedCurve = _selectedMode.ModeValue switch
                {
                    Mode.PowerFunction => new PowerFitter(),
                    Mode.Exponential => new ExponentialFitter(),
                    _ => new LinearFitter()
                };

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
            try
            {
                foreach (var point in _fileReader.GetAllPoints(filePath))
                {
                    Points.Add(new DataPoint(
                      point.X, point.Y));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
