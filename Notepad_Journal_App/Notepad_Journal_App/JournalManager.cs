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
          }

          public void Remove(T item)
          {
               _data.Remove(item);
          }

          public void RemoveById(string id)
          {
               var itemToRemove = _data.FirstOrDefault(item => (item as dynamic).ID == id);
               if (itemToRemove != null)
               {
                    _data.Remove(itemToRemove);
               }
          }
     }

}
