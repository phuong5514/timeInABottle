using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeInABottle.Models;

namespace TimeInABottle.Views
{
    public partial class UpcomingTasksView : Window
    {
        // Constructor
        public UpcomingTasksView()
        {
            InitializeComponent();
            LoadUpcomingTasks();
        }

        public ListBox UpcomingTasksList // Change the type from object to ListBox
        {
            get;
            private set;
        }

        private void InitializeComponent() => throw new NotImplementedException();

        // Method to load sample tasks into the ListBox
        private void LoadUpcomingTasks()
        {
            // Sample data, replace with actual data from your model or service
            List<TaskItem> tasks = new List<TaskItem>
                {
                    new TaskItem { Name = "Họp với khách hàng", DueDate = DateTime.Now.AddDays(2), Description = "Thảo luận về dự án mới." },
                    new TaskItem { Name = "Hoàn thành báo cáo", DueDate = DateTime.Now.AddDays(1), Description = "Chuẩn bị báo cáo tài chính quý 3." },
                    new TaskItem { Name = "Kiểm tra email", DueDate = DateTime.Now.AddDays(3), Description = "Kiểm tra các email công việc quan trọng." }
                };

            // Binding the task list to the ListBox
            UpcomingTasksList.ItemsSource = tasks;
        }
    }

    // Sample TaskItem model, replace with your actual model
    public class TaskItem
    {
        public string Name
        {
            get; set;
        }
        public DateTime DueDate
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
    }
}
