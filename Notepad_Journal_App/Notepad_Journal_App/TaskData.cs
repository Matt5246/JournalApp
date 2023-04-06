using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Notepad_Journal_App
{
     public class TaskData : INotifyPropertyChanged
     {
          private string id;
          private string imagePath;
          private DateTime dueDate;
          private string taskDescription;
          private RelayCommand deleteCommand;

          public event PropertyChangedEventHandler PropertyChanged;

          public string ID
          {
               get { return id; }
               set { id = value; OnPropertyChanged(); }
          }

          public string ImagePath
          {
               get { return imagePath; }
               set { imagePath = value; OnPropertyChanged(); }
          }

          public DateTime DueDate
          {
               get { return dueDate; }
               set { dueDate = value; OnPropertyChanged(); }
          }

          public string TaskDescription
          {
               get { return taskDescription; }
               set { taskDescription = value; OnPropertyChanged(); }
          }

          public RelayCommand DeleteCommand
          {
               get { return deleteCommand; }
               set { deleteCommand = value; OnPropertyChanged(); }
          }

          public TaskData(TaskManager taskManager)
          {
               ID = id;
               DeleteCommand = new RelayCommand(param =>
               {
                    if (taskManager != null)
                    {
                         taskManager.RemoveTaskById(ID);
                    }
               });
          }

          protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
          {
               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
          }
     }

     public class RelayCommand : ICommand
     {
          private readonly Action<object> _execute;
          private readonly Func<object, bool> _canExecute;

          public RelayCommand(Action<object> execute) : this(execute, null)
          {
          }

          public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
          {
               _execute = execute ?? throw new ArgumentNullException(nameof(execute));
               _canExecute = canExecute;
          }

          public bool CanExecute(object parameter)
          {
               return _canExecute == null || _canExecute(parameter);
          }

          public void Execute(object parameter)
          {
               _execute(parameter);
          }

          public event EventHandler CanExecuteChanged
          {
               add { CommandManager.RequerySuggested += value; }
               remove { CommandManager.RequerySuggested -= value; }
          }
     }


}
