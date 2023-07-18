virtual class BaseTimer()
{
  // Delay and execute a ExecutTask?
  // thread
  internal LOGIC FOR WAIT()
  {

    TimeSpan await ExecuteTask();
  }

  public override ExecutTask() {}

  // UPDATE THREAD
  // DISPOSE
}

public class ATaskTimer: BaseTimer
{

    public override ExecutTask()
    {


    }
}

// ################################

// Manager
public class ManagerTimer: IManagerTimer
{
    public ManagerTimer()
    {


    }

    // 1 thread
    // N thread as registers
}

public interface ITimerObject // ITimerExecuter
{
    // identifer, defaultTime
    Task ExecutableTask();
    Task FailureMethod();
    // TimeSpan / DateTime
    Task<TimeSpan> CalculateNextTime();
}

public class ATimerImplementation: IDispose, ITimerObject
{
  // unique identifier
    public ATimerImplementation(IManagerTimer timers)
    {

        // ITimerObject timer = new TimerObject();
        timers.Register(this, initialDueTime);
    }


    // TimeSpan
    public Task ExecutableTask()
    {
        // .,...
        // return time;
        Console.Writeline();
        UiElement.Update();

        // txtMessage.Text = "Update value content"; // FAILS, it is not in dispachter context
        Dispatcher.Invoke(() => txtMessage.Text = "Update value content";); // WORKS

        return Task.__Finished;
    }

    public Task FailureMethod()
    {


    }

    public Task<TimeSpan> CalculateNextTime()
    {
        return calculatedNewTime;
    }

    //         timers.RemoveAll();

    public void Dispose()
    {
        timers.UnRegister(this, initialDueTime);
    }
}

var t1 = new ATimerImplementation(initialDueTime); // thread
var t2 = new ATimerImplementation(initialDueTime); // thread
var t3 = new ATimerImplementation(initialDueTime); // thread
var t4 = new ATimerImplementation(initialDueTime); // thread

while(cancel.IsCanceled) {
    ...
    // Thread.Sleep(500);
    Task.Delay(500).Wait();
    ExecuteTask();
}
if (cancel.IsCanceled)
{



}
