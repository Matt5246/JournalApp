using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad_Journal_App
{
     public class ToDoListData
     {
          private string id;
          public string Task { get; set; }
          public DateTime DueDate { get; set; }
          public bool IsCompleted { get; set; }
          public string ID
          {
               get { return id; }
               set { id = value; }
          }
     }
     public class TodayTaskData
     {
          private string id;
          public string Description { get; set; }
          public DateTime DueDate { get; set; }
          public string ID
          {
               get { return id; }
               set { id = value; }
          }
     }
}
