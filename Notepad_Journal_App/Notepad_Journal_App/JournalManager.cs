using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Notepad_Journal_App
{
     public class DataManager<T>
     {
          private readonly string _filePath;
          private ObservableCollection<T> _data;

          public ObservableCollection<T> Data
          {
               get { return _data; }
          }

          public DataManager(string filePath)
          {
               _filePath = filePath;
               _data = LoadFromFile();
               _data.CollectionChanged += Data_CollectionChanged;
          }

          private ObservableCollection<T> LoadFromFile()
          {
               try
               {
                    string jsonData = File.ReadAllText(_filePath);
                    var deserializedData = JsonConvert.DeserializeObject<ObservableCollection<T>>(jsonData);
                    var dueDateProp = typeof(T).GetProperty("DueDate");
                    if (dueDateProp != null)
                    {
                         deserializedData = new ObservableCollection<T>(deserializedData.OrderBy(t => dueDateProp.GetValue(t, null)));
                    }

                    var idProp = typeof(T).GetProperty("ID");
                    if (idProp != null)
                    {
                         foreach (var item in deserializedData)
                         {
                              var deleteCommandProp = typeof(T).GetProperty("DeleteCommand");
                              if (deleteCommandProp != null)
                              {
                                   var deleteCommand = new RelayCommand(param =>
                                   {
                                        if (this != null)
                                        {
                                             var idValue = idProp.GetValue(item)?.ToString();
                                             if (!string.IsNullOrEmpty(idValue))
                                             {
                                                  this.RemoveTaskById(idValue);
                                             }
                                        }
                                   });

                                   deleteCommandProp.SetValue(item, deleteCommand);
                              }
                         }
                    }


                    return new ObservableCollection<T>(deserializedData);
               }
               catch (Exception ex)
               {
                    // Log the exception details
                    Console.WriteLine($"An error occurred while loading data from file {_filePath}:");
                    Console.WriteLine(ex.ToString());
                    return new ObservableCollection<T>();
               }
          }

          private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
          {
               SaveToFile();
          }

          private void SaveToFile()
          {
               string jsonData = JsonConvert.SerializeObject(_data, Formatting.Indented);
               File.WriteAllText(_filePath, jsonData);
          }

          public void Add(T item)
          {
               _data.Add(item);

               var idProp = typeof(T).GetProperty("ID");
               var deleteProp = typeof(T).GetProperty("DeleteCommand");
               if (idProp != null && deleteProp != null)
               {
                    var id = (string)idProp.GetValue(item);
                    deleteProp.SetValue(item, new RelayCommand(param =>
                    {
                         if (this != null)
                         {
                              this.RemoveTaskById(id);
                         }
                    }));
               }

               var dueDateProp = typeof(T).GetProperty("DueDate");
               // Sort the data
               if (dueDateProp != null)
               {
                    _data = new ObservableCollection<T>(_data.OrderBy(t => dueDateProp.GetValue(t, null)));
               }
               _data.CollectionChanged += Data_CollectionChanged;

          }


          public void RemoveTaskById(string taskId)
          {
               var idProp = typeof(T).GetProperty("ID");
               var taskToRemove = _data.FirstOrDefault(t => idProp.GetValue(t)?.ToString() == taskId);
               if (taskToRemove != null)
               {
                    _data.Remove(taskToRemove);
               }
               _data.CollectionChanged += Data_CollectionChanged;
          }

     }

}
