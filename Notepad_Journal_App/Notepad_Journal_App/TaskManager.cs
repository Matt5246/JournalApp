using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Notepad_Journal_App
{
     public class TaskManager
     {
          private const string FilePath = @"C:\Users\pawelniziolek\Documents\GitHub\JournalApp\Notepad_Journal_App\Notepad_Journal_App\tasks.json";

          private ObservableCollection<TaskData> tasks = new ObservableCollection<TaskData>();

          internal ObservableCollection<TaskData> Tasks
          {
               get { return tasks; }
          }

          public TaskManager()
          {
               LoadTasksFromFile();

               tasks.CollectionChanged += Tasks_CollectionChanged;
          }

          private void LoadTasksFromFile()
          {
               try
               {
                    string tasksJson = File.ReadAllText(FilePath);
                    tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskData>>(tasksJson);
                    // Sort tasks by due date
                    tasks = new ObservableCollection<TaskData>(tasks.OrderBy(t => t.DueDate));
                    foreach (var task in tasks)
                    {
                         task.DeleteCommand = new RelayCommand(param =>
                         {
                              if (this != null)
                              {
                                   this.RemoveTaskById(task.ID);
                              }
                         });
                    }
               }
               catch (Exception ex)
               {
                    // Log the exception details
                    Console.WriteLine("An error occurred while loading tasks from file:");
                    Console.WriteLine(ex.ToString());

               }
          }

          private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
          {
               SaveTasksToFile();
          }

          private void SaveTasksToFile()
          {
               string tasksJson = JsonConvert.SerializeObject(tasks, Formatting.Indented);
               File.WriteAllText(FilePath, tasksJson);
               
          }

          public void AddTask(TaskData task)
          {
               tasks.Add(task);
               tasks.CollectionChanged += Tasks_CollectionChanged;
               LoadTasksFromFile();
          }

          public void RemoveTask(TaskData task)
          {
               tasks.Remove(task);

               tasks.CollectionChanged += Tasks_CollectionChanged;
          }
          public void RemoveTaskById(string id)
          {
               var taskToRemove = tasks.FirstOrDefault(task => task.ID == id);
               if (taskToRemove != null)
               {
                    tasks.Remove(taskToRemove);
               }

               tasks.CollectionChanged += Tasks_CollectionChanged;
          }
     }
}
