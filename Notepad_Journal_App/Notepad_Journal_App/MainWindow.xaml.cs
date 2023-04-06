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

namespace Notepad_Journal_App
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          private const string FilePath = @"C:\Users\matt5\OneDrive\Dokumenty\Github_clones\JournalApp\Notepad_Journal_App\Notepad_Journal_App\tasks.json";

          public string ImagePath { get; private set; }

          private readonly DataManager<TaskData> taskManager;
          private readonly DataManager<JournalData> journalDataManager;

          public DateTime CurrentDate => DateTime.Now;

          public MainWindow()
          {
               InitializeComponent();
               DataContext = new MyViewModel();
               taskManager = new DataManager<TaskData>(@"C:\Users\pawelniziolek\Documents\GitHub\JournalApp\Notepad_Journal_App\Notepad_Journal_App\tasks.json");
               journalDataManager = new DataManager<JournalData>(@"C:\\Users\\pawelniziolek\\Documents\\GitHub\\JournalApp\\Notepad_Journal_App\\Notepad_Journal_App\\journalData.json");


               this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBE4DE"));
               TaskListBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBE4DE"));
               TaskListBox.ItemsSource = taskManager.Data;

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
                    TaskListBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBE4DE"));
               }
               else if (menuItem.Header.ToString() == "Light Blue")
               {
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F6FF"));
                    TaskListBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F6FF"));
               }
               else if (menuItem.Header.ToString() == "Grey")
               {
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
                    TaskListBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE"));
               }
               else if (menuItem.Header.ToString() == "White")
               {
                    this.Background = Brushes.White;
                    TaskListBox.Background = Brushes.White;
               }
               else if (menuItem.Header.ToString() == "Blue")
               {
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A98B9"));
                    TaskListBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A98B9"));
               }
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
                    Date = date,
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
     }
     public class DateTimeToBooleanConverter : IValueConverter
     {
          public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
          {
               var today = "Green";
               var overdue = "Red";
               var other = "Yellow";

               DateTime dueDate = (DateTime)value;

               if (dueDate.Date == DateTime.Now.Date)
               {
                    return today;
               }
               else if (dueDate < DateTime.Now)
               {
                    return overdue;
               }
               else if (dueDate < DateTime.Now.AddDays(7) & dueDate > DateTime.Now)
               {
                    return other;
               }
               else
               {
                    return false;
               }
          }

          public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
          {
               throw new NotImplementedException();
          }
     }
}
