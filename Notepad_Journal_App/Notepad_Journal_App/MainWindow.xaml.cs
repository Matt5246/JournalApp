using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Notepad_Journal_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FilePath = @"C:\Users\matt5\OneDrive\Dokumenty\Github_clones\JournalApp\Notepad_Journal_App\Notepad_Journal_App\tasks.json";

        public string ImagePath { get; private set; }

        ObservableCollection<TaskData> tasks = new ObservableCollection<TaskData>();


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MyViewModel();
            DataContext = new TaskData();

            // Load tasks from tasks.json file
            LoadTasksFromFile();

            // Set the ItemsSource property of the TaskListBox to the tasks collection
            TaskListBox.ItemsSource = tasks;

            // Register the Tasks_CollectionChanged method as a handler for the ObservableCollection.CollectionChanged event
            tasks.CollectionChanged += Tasks_CollectionChanged;
        }
        private void LoadTasksFromFile()
        {
            try
            {
                // Read the JSON string from the tasks.json file
                string tasksJson = File.ReadAllText("tasks.json");

                // Deserialize the JSON string to a list of TaskItem objects
                tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskData>>(tasksJson);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tasks: " + ex.Message);
            }
        }
        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Save changes to the tasks.json file whenever the collection changes
            SaveTasksToFile();
        }
        public class MyViewModel
        {
            public DateTime DueDate { get; set; }
            public string TaskDescription { get; set; }
        }

        private void MenuBackground_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            if (menuItem.Header.ToString() == "Mint")
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBE4DE"));
            }
            else if (menuItem.Header.ToString() == "Light Blue")
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F6FF"));
            }
            else if (menuItem.Header.ToString() == "Grey")
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
            }
            else if (menuItem.Header.ToString() == "White")
            {
                this.Background = Brushes.White;
            }
            else if (menuItem.Header.ToString() == "Blue")
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A98B9"));
            }

        }
        private void MenuFontColor_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                string color = item.Header.ToString(); // get the selected color from the MenuItem header

                // set the font color of the application
                SolidColorBrush brush = new SolidColorBrush();
                switch (color)
                {
                    case "Mint":
                        brush.Color = Color.FromRgb(62, 182, 171);
                        break;
                    case "Light Blue":
                        brush.Color = Color.FromRgb(135, 206, 250);
                        break;
                    case "Grey":
                        brush.Color = Color.FromRgb(169, 169, 169);
                        break;
                    case "Blue":
                        brush.Color = Color.FromRgb(70, 130, 180);
                        break;
                    case "White":
                        brush.Color = Colors.White;
                        break;
                    default:
                        break;
                }
                // create a new style for TextBlock elements
                Style style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.ForegroundProperty, brush));

                // apply the style to all TextBlock elements in the application
                Application.Current.Resources[typeof(TextBlock)] = style;
            }
        }
        private void MenuFontSize_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = e.Source as MenuItem;
            string size = clickedMenuItem.Header.ToString().ToLower();

            switch (size)
            {
                case "small":
                    SetTextBlockFontSize(12);
                    break;
                case "medium":
                    SetTextBlockFontSize(16);
                    break;
                case "large":
                    SetTextBlockFontSize(20);
                    break;
                default:
                    break;
            }

        }
        private void SetTextBlockFontSize(double size)
        {
            Style style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.FontSizeProperty, size));

            // apply the style to all TextBlock elements in the application
            Application.Current.Resources[typeof(TextBlock)] = style;
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
                ImagePreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ImagePathTextBox.Text))
            {
                ImagePath = ImagePathTextBox.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select an image.");
            }
        }
        private void UploadButton_State(object sender, RoutedEventArgs e)
        {
            if (ImagePathTextBox.Text.Length>7)
            {
                UploadButton.IsEnabled = true;
            }
            else
            {
                UploadButton.IsEnabled = false;
            }
        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ImagePathTextBox.Text))
            {
                try
                {
                    ImagePreview.Source = new BitmapImage(new Uri(ImagePathTextBox.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an image.");
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
          }
          private void SubmitTaskButton_Click(object sender, RoutedEventArgs e)
          {
               // Collect the form data
               var taskData = new TaskData
               {
                    ImagePath = ImagePathTextBox.Text,
                    DueDate = (DateTime)DatePicker.SelectedDate,
                    TaskDescription = TaskDescriptionTextBox.Text
               };

               // Add the new task to the tasks collection
               tasks.Add(taskData);

               // Save the tasks to the tasks.json file
               SaveTasksToFile();

               // Update the TaskListBox
               TaskListBox.ItemsSource = tasks;
          }
          private void SaveTasksToFile()
          {
               try
               {
                    // Serialize the ObservableCollection<TaskItem> to a JSON string
                    string tasksJson = JsonConvert.SerializeObject(tasks, Formatting.Indented);

                    // Write the JSON string to the tasks.json file
                    File.WriteAllText("tasks.json", tasksJson);
               }
               catch (Exception ex)
               {
                    MessageBox.Show("Error saving tasks: " + ex.Message);
               }
          }

     }
}
