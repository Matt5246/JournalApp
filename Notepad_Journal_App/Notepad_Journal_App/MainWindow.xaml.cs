using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Notepad_Journal_App
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          public string ImagePath { get; private set; }

          private readonly DataManager<TaskData> taskManager;
          private readonly DataManager<JournalData> journalDataManager;
          private readonly DataManager<TodayTaskData> dailyTasksManager;
          private readonly DataManager<ToDoListData> toDoListDataManager;

          public DateTime CurrentDate => DateTime.Now;

          public MainWindow()
          {
               InitializeComponent();
               DataContext = new MyViewModel();
               
               //Console.WriteLine(Path.Combine(dataFolderPathWithDoubleBackslashes, "journal.json")); //Path.Combine(dataFolderPathWithDoubleBackslashes, "\journal.json");
               taskManager = new DataManager<TaskData>("C:\\Users\\pawelniziolek\\Documents\\GitHub\\JournalApp\\Notepad_Journal_App\\Notepad_Journal_App\\Data\\tasks.json");
               journalDataManager = new DataManager<JournalData>("C:\\Users\\pawelniziolek\\Documents\\GitHub\\JournalApp\\Notepad_Journal_App\\Notepad_Journal_App\\Data\\journal.json");
               dailyTasksManager = new DataManager<TodayTaskData>("C:\\Users\\pawelniziolek\\Documents\\GitHub\\JournalApp\\Notepad_Journal_App\\Notepad_Journal_App\\Data\\dailyTasks.json");
               toDoListDataManager = new DataManager<ToDoListData>("C:\\Users\\pawelniziolek\\Documents\\GitHub\\JournalApp\\Notepad_Journal_App\\Notepad_Journal_App\\Data\\toDoList.json");

               TaskListBox.ItemsSource = taskManager.Data;
               JournalListBox.ItemsSource = journalDataManager.Data;
               ToDoListBox.ItemsSource = toDoListDataManager.Data;
               DailyTaskListBox.ItemsSource = dailyTasksManager.Data;

               // Loading the background colors
               string bgColor1 = Properties.Settings.Default.BackgroundColor1;
               string bgColor2 = Properties.Settings.Default.BackgroundColor2;
               int FontSize = Properties.Settings.Default.FontSize;
               BackGroundChange(bgColor1, bgColor2);
               SetTextBlockFontSize(FontSize);
          }
          private void MenuBackground_Click(object sender, RoutedEventArgs e)
          {
               MenuItem menuItem = e.Source as MenuItem;
               string startColor, endColor;

               switch (menuItem.Header.ToString())
               {
                    case "Mint":
                         startColor = "#CBE4DE";
                         endColor = "#C1F2D1";
                         break;
                    case "Light Blue":
                         startColor = "#E3F6FF";
                         endColor = "#BFE9FF";
                         break;
                    case "Grey":
                         startColor = "#EEEEEE";
                         endColor = "#E0E0E0";
                         break;
                    case "White":
                         startColor = "#FFFFFF";
                         endColor = "#F7F7F7";
                         break;
                    case "Yellow":
                         startColor = "#FCE38A";
                         endColor = "#F38181";
                         break;
                    case "Pink":
                         startColor = "#FFAFCC";
                         endColor = "#FFC8A2";
                         break;
                    case "Purple":
                         startColor = "#A09ABC";
                         endColor = "#F7CAC9";
                         break;
                    case "Green":
                         startColor = "#9BE7A3";
                         endColor = "#00A896";
                         break;
                    case "Orange":
                         startColor = "#FFB86F";
                         endColor = "#FFA69E";
                         break;
                    case "Blue":
                         startColor = "#98D9EA";
                         endColor = "#6FB1C1";
                         break;
                    case "Turquoise":
                         startColor = "#AFDFD9";
                         endColor = "#BEE7E9";
                         break;
                    case "Coral":
                         startColor = "#FF847C";
                         endColor = "#E84A5F";
                         break;
                    case "Lavender":
                         startColor = "#D7BDE2";
                         endColor = "#FADBD8";
                         break;
                    case "Salmon":
                         startColor = "#F1948A";
                         endColor = "#F5B7B1";
                         break;
                    case "Light Green":
                         startColor = "#A2D9CE";
                         endColor = "#E9D8A6";
                         break;
                    case "Teal":
                         startColor = "#80CED7";
                         endColor = "#0077B6";
                         break;
                    case "Beige":
                         startColor = "#F5DEB3";
                         endColor = "#D2B48C";
                         break;
                    default:
                         startColor = "#FFFFFF";
                         endColor = "#F7F7F7";
                         break;
               }

               BackGroundChange(startColor, endColor);
               // Saving the background colors
               Properties.Settings.Default.BackgroundColor1 = startColor; // Replace with your first color
               Properties.Settings.Default.BackgroundColor2 = endColor; // Replace with your second color
               Properties.Settings.Default.Save();

               // Loading the background colors
               string bgColor1 = Properties.Settings.Default.BackgroundColor1;
               string bgColor2 = Properties.Settings.Default.BackgroundColor2;
               BackGroundChange(bgColor1, bgColor2); // Call your function with both colors
          }

          private void BackGroundChange(string startColor, string endColor)
          {
               LinearGradientBrush gradientBrush = new LinearGradientBrush();
               gradientBrush.StartPoint = new Point(0, 0);
               gradientBrush.EndPoint = new Point(1, 1);
               gradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(startColor), 0));
               gradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString(endColor), 1));

               this.Background = gradientBrush;
          }

          private void MenuFontColor_Click(object sender, RoutedEventArgs e)
          {
               MenuItem item = sender as MenuItem;
               if (item != null)
               {
                    string color = item.Header.ToString();
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

                    Style style = new Style(typeof(TextBlock));
                    style.Setters.Add(new Setter(TextBlock.ForegroundProperty, brush));

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
                         Properties.Settings.Default.FontSize = 12;
                         break;
                    case "medium":
                         SetTextBlockFontSize(16);
                         Properties.Settings.Default.FontSize = 16;
                         break;
                    case "large":
                         SetTextBlockFontSize(20);
                         Properties.Settings.Default.FontSize = 20;
                         break;
                    default:
                         break;
               }

               Properties.Settings.Default.Save();
          }
          private void SetTextBlockFontSize(double size)
          {
               Style style = new Style(typeof(TextBlock));
               style.Setters.Add(new Setter(TextBlock.FontSizeProperty, size));

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
                    TaskDescription = TaskDescriptionTextBox.Text,
                    ID = Guid.NewGuid().ToString()
               };

               // Add the new task to the list of tasks
               taskManager.Add(taskData);
               TaskListBox.ItemsSource = taskManager.Data;

               // Clear the text box and date picker
               TaskDescriptionTextBox.Clear();
               DatePicker.SelectedDate = null;
          }

          private void Journal_Button_Click(object sender, RoutedEventArgs e)
          {
               // Get the values from the input controls
               string title = JournalTitleTextBox.Text;
               DateTime date = JournalDatePicker.SelectedDate.Value;
               string mood = ((ComboBoxItem)MoodComboBox.SelectedItem).Content.ToString();
               string entry = JournalEntryTextBox.Text;

               // Create a new journal entry
               JournalData newEntry = new JournalData
               {
                    Title = title,
                    DueDate = date,
                    Mood = mood,
                    Entry = entry
               };

               // Add the new entry to the data manager
               journalDataManager.Add(newEntry);

               // Clear the input controls
               JournalTitleTextBox.Text = "";
               JournalDatePicker.SelectedDate = null;
               MoodComboBox.SelectedIndex = -1;
               JournalEntryTextBox.Text = "";
          }

          private void ToDoList_Button_Click(object sender, RoutedEventArgs e)
          {
               // Get the task and due date from the text box and date picker
               string task = ToDoListTitleTextBox.Text;
               DateTime? dueDate = ToDoListDatePicker.SelectedDate.Value;

               // Make sure the task text box is not empty
               if (!string.IsNullOrWhiteSpace(task))
               {
                    // Create a new task item and add it to the data manager
                    ToDoListData newTask = new ToDoListData
                    {

                         Task = task,
                         DueDate = dueDate ?? DateTime.MinValue,
                         ID = Guid.NewGuid().ToString(),
                         IsCompleted= false

                    };
                    toDoListDataManager.Add(newTask);

                    // Clear the text box and date picker
                    ToDoListTitleTextBox.Text = string.Empty;
                    ToDoListDatePicker.SelectedDate = null;
               }
          }

          private void DailyTasks_Button_Click(object sender, RoutedEventArgs e)
          {
               string description = TodayTaskTextBox.Text;
               if (!string.IsNullOrWhiteSpace(description))
               {
                    // Create a new task item and add it to the data manager
                    TodayTaskData newTask = new TodayTaskData
                    {
                         Description = description,
                         ID = Guid.NewGuid().ToString(),
                         IsCompleted= false
                    };
                    dailyTasksManager.Add(newTask);

                    // Clear the text box and date picker
                    TodayTaskTextBox.Text = string.Empty;
               }
          }

     }
}
