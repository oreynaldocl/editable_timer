using EditableTimer;
using System.Windows.Controls;

namespace TimerDisplayer.Executers
{
    public class Logger : ILogger
    {
        private readonly TextBlock _textBlock;

        public Logger(TextBlock textBlock)
        {
            _textBlock = textBlock;
        }
        public void Log(string message)
        {
            _textBlock.Text += $"\n{message}";
        }
    }
}
