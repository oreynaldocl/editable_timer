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
        private Dictionary<int, WorkerExecuter> timers;
        private int currentId = 1000;
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

            timers = new Dictionary<int, WorkerExecuter>();

            _manager = new TimerManager(_logText);
        }

        private void Add_Timer(object sender, RoutedEventArgs e)
        {
            int interval;
            if (!Int32.TryParse(TimeIntervalTextBox.Text, out interval))
            {
                interval = 15;
            }
            StackPanel newPanel = makeTimerPanel(currentId, interval);
            Label triggeredLabel = (Label)newPanel.Children[5];
            WorkerExecuter worker = new WorkerExecuter(_logText, Dispatcher, newPanel, currentId, interval);
            _manager.RegisterTimer(worker, TimeSpan.FromSeconds(interval));
            timers.Add(currentId, worker);
            timerPanel.Children.Add(newPanel);
            currentId++;
        }

        private void Delete_Timer(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            StackPanel parent = (StackPanel)(b.Parent);
            int id = Int32.Parse(((Label)parent.Children[0]).Content.ToString());
            WorkerExecuter worker = timers[id];
            timerPanel.Children.Remove(worker.panel);
            _manager.UnregisterTimer(worker);
            timers.Remove(id);

        }

        private void Reset_Timer(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            StackPanel parent = (StackPanel)(b.Parent);
            int id = Int32.Parse(((Label)parent.Children[0]).Content.ToString());
            WorkerExecuter worker = timers[id];
            int newInterval = Int32.Parse(((TextBox)worker.panel.Children[2]).Text);
            ((Label)worker.panel.Children[4]).Content = newInterval;
            worker.interval = newInterval;
            _manager.ChangeWaitTime(worker, TimeSpan.FromSeconds(newInterval));
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
            deleteButton.Click += Delete_Timer;
            deleteButton.Name = "DeleteButton";

            TextBox tBox = new TextBox();
            tBox.Width = 100;
            tBox.Margin = defaultThickness;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Name = "TimeResetTextBox";
            tBox.Text = timeInterval.ToString();

            Button resetButton = new Button();
            resetButton.Width = 50;
            resetButton.VerticalAlignment = VerticalAlignment.Center;
            resetButton.Margin = defaultThickness;
            resetButton.Content = "Set";
            resetButton.Click += Reset_Timer;
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
