namespace study_buddy_mobile.Pages;

using System.Timers;
using System;
using Microsoft.Maui.Controls;
public partial class TimerPage : ContentPage
{
	private Timer timer;
	private int secondsElapsed = 0;
	private bool isRunning = false;

	public TimerPage()
	{
		InitializeComponent();

		timer = new Timer();
		timer.Elapsed += OnTimedEvent;
	}

	private void OnTimedEvent(object sender, EventArgs e)
	{
		if (!isRunning) return;

		secondsElapsed++;

		MainThread.BeginInvokeOnMainThread(new Action(() =>
		{
			TimeSpan time = TimeSpan.FromSeconds(secondsElapsed);
			TimerLabel.Text = time.ToString(@"mm\:ss");
		}));
	}
    private void StartButton_Clicked(object sender, EventArgs e)
    {
        isRunning = true;
        timer.Start();
    }

    private void PauseButton_Clicked(object sender, EventArgs e)
    {
        isRunning = false;
        timer.Stop();
    }

    private void ResetButton_Clicked(object sender, EventArgs e)
    {
        isRunning = false;
        timer.Stop();
        secondsElapsed = 0;
        TimerLabel.Text = "00:00";
    }
}