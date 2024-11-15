using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeInABottle.Services;

namespace TimeInABottle.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private const string V = "Reminder for test task";

        [TestMethod]
        public void TestIntegrationOfFeatures()
        {
            var taskService = new TaskService();
            var notificationService = new NotificationService();

            // Thêm một công việc mới
            var task = taskService.AddTask("Test task", DateTime.Now.AddDays(1));
            notificationService.CreateNotification(V, task.Deadline);

            // Kiểm tra rằng thông báo đã được tạo ra
            var notifications = notificationService.GetNotifications();
            Assert.IsTrue(condition: notifications.Any(static n => n.Message.Contains("test task")));
        }
    }
}
