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

          private void ToggleBackgroundColor(object sender, RoutedEventArgs e)
          {
               if (isDark)
               {
                    // Change background color to white
                    this.Background = Brushes.White;
                    isDark = false;
               }
               else
               {
                    // Change background color to dark
                    this.Background = Brushes.DarkGray;
                    isDark = true;
               }
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
