using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TouchSamples.PhotoViewer.Common
{
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> m_Execute;
        readonly Predicate<object> m_CanExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            m_Execute = execute;
            m_CanExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return m_CanExecute == null ? true : m_CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            m_Execute(parameter);
        }

        #endregion // ICommand Members
    }
}