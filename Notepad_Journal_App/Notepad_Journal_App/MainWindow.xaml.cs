using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace Notepad_Journal_App
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          private const string FilePath = @"C:\Users\pawelniziolek\source\repos\Notepad_Journal_App\Notepad_Journal_App\tasks.json"; // replace with your desired file path

          private bool isDark = false;

          public string ImagePath { get; private set; }

          public MainWindow()
          {
               InitializeComponent();
               DataContext = new MyViewModel();
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
               }else if (menuItem.Header.ToString() == "Blue")
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
     }
}
