using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarTracking.MobileApp.Models;

namespace CarTracking.MobileApp.ViewModels;

public class LogViewModel : INotifyPropertyChanged
{
    private static readonly Lazy<LogViewModel> _instance = new(() => new LogViewModel());
    public static LogViewModel Instance => _instance.Value;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Log> Logs { get; set; } = new();
    public ObservableCollection<string> LogsString { get; set; } = new();

    public int RecordsKeepCount { get; set; }


    public void AddLog(Log logToAdd)
    {
        CheckLogsCount();

        Logs.Insert(0, logToAdd);
        LogsString.Insert(0, $"Sending result: {logToAdd.Text} at: {logToAdd.Date.ToLongTimeString()}");
    }

    public void AddTestNotificationLog(Log logToAdd)
    {
        CheckLogsCount();

        Logs.Insert(0, logToAdd);
        LogsString.Insert(0, $"Test Notification request: {logToAdd.Text} at: {logToAdd.Date.ToLongTimeString()}");
    }

    private void CheckLogsCount()
    {
        if (Logs.Count <= RecordsKeepCount) return;
        Logs.RemoveAt(Logs.Count - 1);
        LogsString.RemoveAt(LogsString.Count - 1);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdateKeepCount(int newCount)
    {
        RecordsKeepCount = newCount;
        if (Logs.Count > newCount)
        {
            Logs.Clear();
            LogsString.Clear();
        }

        OnPropertyChanged(nameof(RecordsKeepCount));
    }
}