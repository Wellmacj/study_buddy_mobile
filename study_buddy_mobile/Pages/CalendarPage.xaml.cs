namespace study_buddy_mobile.Pages;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
public partial class CalendarPage : ContentPage
{
    private ObservableCollection<TodoItem> AllTasks = new();
    private DateTime selectedDate;
    public CalendarPage()
	{
		InitializeComponent();

        LoadTasksAsync().ContinueWith(_ =>
        {
            // Ensure UI updates on main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                selectedDate = CalendarPicker.Date;
                SelectedDateLabel.Text = $"Tasks for {selectedDate:MMMM dd, yyyy}";
                LoadTasksForSelectedDate();
            });
        });
    }

    private void CalendarPicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        selectedDate = e.NewDate;
        SelectedDateLabel.Text = $"Tasks for {selectedDate:MMMM dd, yyyy}";
        LoadTasksForSelectedDate();
    }


    private async void AddTask_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            AllTasks.Add(new TodoItem
            {
                Title = TaskEntry.Text.Trim(),
                Date = selectedDate
            });

            TaskEntry.Text = "";
            LoadTasksForSelectedDate();

            await SaveTasksAsync();
        }
    }


    private void LoadTasksForSelectedDate()
    {
        var filtered = AllTasks
            .Where(task => task.Date.Date == selectedDate.Date)
            .ToList();

        TaskListView.ItemsSource = filtered;
    }
    private async Task SaveTasksAsync()
{
    var tasksFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
    var json = JsonSerializer.Serialize(AllTasks);
    await File.WriteAllTextAsync(tasksFile, json);
}

    private async Task LoadTasksAsync()
    {
        var tasksFile = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        if (File.Exists(tasksFile))
        {
            var json = await File.ReadAllTextAsync(tasksFile);
            var loadedTasks = JsonSerializer.Deserialize<ObservableCollection<TodoItem>>(json);
            if (loadedTasks != null)
            {
                AllTasks = loadedTasks;
            }
        }
    }
    private async void DeleteTask_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is TodoItem taskToDelete)
        {
            AllTasks.Remove(taskToDelete);
            LoadTasksForSelectedDate();
            await SaveTasksAsync();
        }
    }

    private async void TimerClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TimerPage());
    }
    private async void HomeButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

}