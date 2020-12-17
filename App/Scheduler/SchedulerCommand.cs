namespace App
{
    public class SchedulerCommand
    {
        public SchedulerCommandType CommandType;

        public SchedulerCommand(SchedulerCommandType schedulerCommandType)
        {
            CommandType = schedulerCommandType;
        }
    }
}