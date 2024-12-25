using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TimeInABottle.Core.Models.Tasks;
internal class TaskContext : DbContext
{
    public DbSet<ITask> Tasks { get; set; }

    public string DbPath
    {
        get;
    }

    // TODO: need to move the db name to a secret manager
    private readonly string _dbName = "taskStorage.db";

    public TaskContext()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        var folder = AppContext.BaseDirectory;
        //var path = Environment.GetFolderPath(folder);
        //DbPath = System.IO.Path.Join(path, _dbName);
        DbPath = System.IO.Path.Combine(folder, _dbName);
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ITask>().ToTable("Tasks");
        var taskTypes = new[] { typeof(DailyTask), typeof(WeeklyTask), typeof(MonthlyTask), typeof(NonRepeatedTask), typeof(DerivedTask) };
        foreach (var taskType in taskTypes)
        {
            modelBuilder.Entity(taskType).ToTable($"{taskType.Name}s");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
