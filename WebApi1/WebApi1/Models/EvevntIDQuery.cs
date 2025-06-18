public class EventIDQueryData
{
    public string ComputerName { get; }
    public List<string> EventIDs { get; }
    public string LogName { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public string EventType { get; }
    public string EventSource { get; }

     EventIDQueryData(
        string computerName,
        List<string> eventIDs,
        string logName,
        DateTime startDate,
        DateTime endDate,
        string eventType,
        string eventSource)
    {
        ComputerName = computerName;
        EventIDs = eventIDs;
        LogName = logName;
        StartDate = startDate;
        EndDate = endDate;
        EventType = eventType;
        EventSource = eventSource;
    }
}
