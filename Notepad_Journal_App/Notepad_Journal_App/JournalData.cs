﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Notepad_Journal_App
{
     public class JournalData
     {
          public string Title { get; set; }
          public DateTime Date { get; set; }
          public string Mood { get; set; }
          public string Entry { get; set; }
     }
}
