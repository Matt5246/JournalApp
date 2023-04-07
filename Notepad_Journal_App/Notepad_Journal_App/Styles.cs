using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Notepad_Journal_App
{
     public class MyViewModel
     {
          public DateTime DueDate { get; set; }
          public string TaskDescription { get; set; }
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
