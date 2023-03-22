﻿using System;
using System.Windows.Input;

namespace Notepad_Journal_App
{
     public class TaskData
     {

          public string ImagePath { get; set; }
          public DateTime DueDate { get; set; }
          public string TaskDescription { get; set; }
          public ICommand DeleteCommand { get; set; }
          public TaskData(TaskManager taskManager)
          {
               DeleteCommand = new RelayCommand(param => taskManager.RemoveTask(this));
          }
     }
     public class RelayCommand : ICommand
     {
          private readonly Action<object> _execute;
          private readonly Func<object, bool> _canExecute;

          public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
          {
               _execute = execute ?? throw new ArgumentNullException(nameof(execute));
               _canExecute = canExecute;
          }

          public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

          public void Execute(object parameter) => _execute(parameter);

          public event EventHandler CanExecuteChanged;
     }
}
