using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfVisualizer.Util;

namespace WpfVisualizer.ViewModel
{
    internal class Waveform : INotifyPropertyChanged
    {
        #region Components

        private Source Source;
        private Display Display;
        private DispatcherTimer Timer = new();

        #endregion

        #region Constructor

        public Waveform()
        {
            Source = new Source();
            Display = new Display(Source.Provider);
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            SetupTimer();
        }

        #endregion

        #region Timer

        public event Action<Bitmap>? UpdateCanvas = null;

        private void SetupTimer()
        {
            Timer.Interval = TimeSpan.FromMilliseconds(1000 / 30);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateCanvas?.Invoke(Display.RenderFrame());
        }

        #endregion

        #region Change handler

        public event PropertyChangedEventHandler PropertyChanged = null;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Buttons

        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        public Visibility StartButtonVisibility {
            get { return !Timer.IsEnabled ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility StopButtonVisibility {
            get { return Timer.IsEnabled ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region Logic

        private void Start()
        {
            Timer.Start();
            Source.Start();
            OnStartStop();
        }

        private void Stop()
        {
            Timer.Stop();
            Source.Stop();
            OnStartStop();
        }

        private void OnStartStop()
        {
            OnPropertyChanged(nameof(StartButtonVisibility));
            OnPropertyChanged(nameof(StopButtonVisibility));
        }

        #endregion
    }
}
