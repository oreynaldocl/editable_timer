using EditableTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimerDisplayer.Executers;

namespace TimerDisplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Thickness defaultThickness = new Thickness();
        List<object> timers;

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

            TimerManager manager = new TimerManager(_logText);
            SimpleExecuter executer1 = new SimpleExecuter(0, _logText);
            manager.RegisterTimer(executer1, TimeSpan.FromSeconds(3));
            ClockUpdateExecuter clockUpdater = new ClockUpdateExecuter(_logText, Dispatcher, CurrentTimeLabel);
            manager.RegisterTimer(clockUpdater, TimeSpan.FromSeconds(1));
        }

        private void Add_Timer(object sender, RoutedEventArgs e)
        {
            int interval;
            if(!Int32.TryParse(TimeIntervalTextBox.Text, out interval))
            {
                interval = 15;
            }
            StackPanel newPanel = makeTimerPanel(timers.Count, interval);
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

            TextBox tBox = new TextBox();
            tBox.Width = 100;
            tBox.Margin = defaultThickness;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Name = "TimeResetTextBox";

            Button button = new Button();
            button.Width = 50;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Margin = defaultThickness;
            button.Content = "Set";
            button.Name = "SetButton";

            Label intervalLabel = new Label();
            intervalLabel.Width = 100;
            intervalLabel.VerticalAlignment = VerticalAlignment.Center;
            intervalLabel.Margin = defaultThickness;
            intervalLabel.Content = timeInterval.ToString();
            intervalLabel.Name = "triggerLabel";

            Label triggerLabel = new Label();
            triggerLabel.Width = 100;
            triggerLabel.VerticalAlignment = VerticalAlignment.Center;
            triggerLabel.Margin = defaultThickness;
            triggerLabel.Content = "";
            triggerLabel.Name = "triggered";

            panel.Children.Add(idLabel);
            panel.Children.Add(tBox);
            panel.Children.Add(button);
            panel.Children.Add(intervalLabel);
            panel.Children.Add(triggerLabel);

            return panel;
        }
    }
}
