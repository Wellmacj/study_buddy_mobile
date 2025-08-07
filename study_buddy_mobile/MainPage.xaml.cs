using study_buddy_mobile.Pages;
using System.Threading.Tasks;

namespace study_buddy_mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnTimerClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TimerPage());
        }

        private async void OnCalendarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalendarPage());
        }



    }
}
