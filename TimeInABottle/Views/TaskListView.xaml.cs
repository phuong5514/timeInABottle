using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using TimeInABottle.Models;

namespace TimeInABottle.Views
{
    public partial class TaskListView : Window
    {
        // Constructor
        public TaskListView()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent() => throw new NotImplementedException();

        // Sample data for tasks
        private void LoadTasks()
        {
            var tasks = new List<TaskItem>
                {
                    new TaskItem { Name = "Họp với khách hàng", DueDate = DateTime.Now.AddDays(2), Priority = "Cao", Description = "Thảo luận về dự án mới." },
                    new TaskItem { Name = "Hoàn thành báo cáo", DueDate = DateTime.Now.AddDays(1), Priority = "Trung bình", Description = "Chuẩn bị báo cáo tài chính quý 3." },
                    new TaskItem { Name = "Kiểm tra email", DueDate = DateTime.Now.AddDays(3), Priority = "Thấp", Description = "Kiểm tra các email công việc quan trọng." }
                };

            // Bind tasks to ListBox
            TaskList.ItemsSource = tasks;
        }

        // Event handler for Sort button click
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSortOption = SortByComboBox.SelectedItem as ComboBoxItem;
            if (selectedSortOption != null)
            {
                string sortOption = selectedSortOption.Content.ToString();
                var tasks = TaskList.ItemsSource as List<TaskItem>;

                if (tasks != null)
                {
                    switch (sortOption)
                    {
                        case "Theo mức độ ưu tiên":
                            tasks = tasks.OrderBy(t => t.Priority).ToList();
                            break;
                        case "Theo deadline":
                            tasks = tasks.OrderBy(t => t.DueDate).ToList();
                            break;
                    }

                    // Rebind the sorted task list
                    TaskList.ItemsSource = tasks;
                }
            }
        }
    }

    // Sample TaskItem model, replace with your actual model
    public class TaskItem
    {
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
    }
}
