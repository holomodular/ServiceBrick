using Microsoft.Extensions.Logging;
using ServiceBrick;

namespace Example
{
    public class ExampleExpireProductTimer : TaskTimerHostedService<ExampleExpireProductTask.Detail, ExampleExpireProductTask.Worker>
    {
        public ExampleExpireProductTimer(
            IServiceProvider serviceProvider,
            ILoggerFactory logger) : base(serviceProvider, logger)
        {
        }

        public override TimeSpan TimerTickInterval
        {
            get { return TimeSpan.FromMinutes(2); }
        }

        public override TimeSpan TimerDueTime
        {
            get { return TimeSpan.FromSeconds(30); }
        }

        public override ITaskDetail<ExampleExpireProductTask.Detail, ExampleExpireProductTask.Worker> TaskDetail
        {
            get { return new ExampleExpireProductTask.Detail(); }
        }

        public override bool TimerTickShouldProcessRun()
        {
            return ApplicationBuilderExtensions.BrickStarted && !IsCurrentlyRunning;
        }
    }
}