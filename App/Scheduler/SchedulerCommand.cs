namespace App
{
    public class SchedulerCommand
    {
        public SchedulerCommandType CommandType { get; }

        public SchedulerCommand(SchedulerCommandType schedulerCommandType)
        {
            CommandType = schedulerCommandType;
        }
    }
}