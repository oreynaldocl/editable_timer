using EditableTimer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TimerDisplayer.Executers;

namespace TimerDisplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Thickness defaultThickness = new Thickness();
        private List<object> timers;
        private readonly TimerManager _manager;

        private LoggerText _logText = new LoggerText();
        public LoggerText LogText => _logText;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            defaultThickness.Bottom = 3;
            defaultThickness.Left = 3;
            defaultThickness.Right = 3;
            defaultThickness.Top = 3;

            timers = new List<object>();

            _manager = new TimerManager(_logText);
            ClockUpdateExecuter clockUpdater = new ClockUpdateExecuter(_logText, Dispatcher, CurrentTimeLabel);
            _manager.RegisterTimer(clockUpdater, TimeSpan.FromSeconds(5));
        }

        private void Add_Timer(object sender, RoutedEventArgs e)
        {
            int interval;
            if (!Int32.TryParse(TimeIntervalTextBox.Text, out interval))
            {
                interval = 15;
            }
            StackPanel newPanel = makeTimerPanel(timers.Count, interval);
            Label triggeredLabel = (Label)newPanel.Children[5];
            //WorkerExecuter worker = new WorkerExecuter(_logText, Dispatcher, triggeredLabel, timers.Count + 1000, interval);
            //_manager.RegisterTimer(worker, TimeSpan.FromSeconds(interval));
            timers.Add(timers.Count);
            timerPanel.Children.Add(newPanel);
        }

        private StackPanel makeTimerPanel(int count, int timeInterval)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Height = 30;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.VerticalAlignment = VerticalAlignment.Center;

            Label idLabel = new Label();
            idLabel.Width = 100;
            idLabel.VerticalAlignment = VerticalAlignment.Center;
            idLabel.Margin = defaultThickness;
            idLabel.Content = count.ToString();
            idLabel.Name = "idLabel";

            Button deleteButton = new Button();
            deleteButton.Width = 25;
            deleteButton.VerticalAlignment = VerticalAlignment.Center;
            deleteButton.Margin = defaultThickness;
            deleteButton.Content = "X";
            deleteButton.Name = "DeleteButton";

            TextBox tBox = new TextBox();
            tBox.Width = 100;
            tBox.Margin = defaultThickness;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Name = "TimeResetTextBox";

            Button resetButton = new Button();
            resetButton.Width = 50;
            resetButton.VerticalAlignment = VerticalAlignment.Center;
            resetButton.Margin = defaultThickness;
            resetButton.Content = "Set";
            resetButton.Name = "SetButton";

            Label intervalLabel = new Label();
            intervalLabel.Width = 100;
            intervalLabel.VerticalAlignment = VerticalAlignment.Center;
            intervalLabel.Margin = defaultThickness;
            intervalLabel.Content = timeInterval.ToString();
            intervalLabel.Name = "intervalLabel";

            Label triggerLabel = new Label();
            triggerLabel.Width = 100;
            triggerLabel.VerticalAlignment = VerticalAlignment.Center;
            triggerLabel.Margin = defaultThickness;
            triggerLabel.Foreground = Brushes.Red;
            triggerLabel.Content = "";
            triggerLabel.Name = "triggeredLabel";

            panel.Children.Add(idLabel);
            panel.Children.Add(deleteButton);
            panel.Children.Add(tBox);
            panel.Children.Add(resetButton);
            panel.Children.Add(intervalLabel);
            panel.Children.Add(triggerLabel);

            return panel;
        }
    }
}
