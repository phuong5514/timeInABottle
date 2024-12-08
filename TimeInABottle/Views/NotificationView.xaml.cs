using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace TimeInABottle.Views
{
    public partial class NotificationView : Window
    {
        // Sample notification data
        private List<string> _notifications;

        public ListBox NotificationList // Change type from object to ListBox
        {
            get;
            private set;
        }
        public object MessageBox
        {
            get;
            private set;
        }

        public NotificationView()
        {
            InitializeComponent();
            LoadNotifications();
        }

        private void InitializeComponent() => throw new NotImplementedException();

        private void LoadNotifications()
        {
            // Add sample notifications
            _notifications = new List<string>
                    {
                        "You have an upcoming event.",
                        "Task 'Complete the report' is still pending.",
                        "Reminder: Submit the document by tomorrow."
                    };

            // Display notifications in ListBox
            NotificationList.ItemsSource = _notifications;
        }

        private void MarkAsReadButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove selected notifications
            var selectedNotifications = NotificationList.SelectedItems.Cast<string>().ToList();

            if (selectedNotifications.Any())
            {
                foreach (var notification in selectedNotifications)
                {
                    _notifications.Remove(notification);
                }
                NotificationList.ItemsSource = null;
                NotificationList.ItemsSource = _notifications;
            }
            else
            {
                ContentDialog noSelectionDialog = new ContentDialog
                {
                    Title = "Notification",
                    Content = "Please select at least one notification to mark as read.",
                    CloseButtonText = "Ok"
                };

                noSelectionDialog.ShowAsync();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
