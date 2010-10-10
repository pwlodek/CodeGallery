using System;
using System.Windows.Input;

namespace ParallelCoordinatesDemo
{
    public sealed class RelayCommand : ICommand
    {
        #region Declarations

        private readonly Predicate<object> _canExecuteMethod;
        private readonly Action<object> _executeMethod;

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        #endregion

        #region Constructor

        public RelayCommand(Action<object> executeMethod)
            : this(executeMethod, null)
        {
        }

        public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
        {

            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod", "Delegate comamnds can not be null");
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            if (_canExecuteMethod == null)
            {
                return true;
            }

            return _canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            if (_executeMethod == null)
            {
                return;
            }

            _executeMethod(parameter);
        }

        #endregion
    }
}

//
//=====================================================================================================
//=====================================================================================================
//=====================================================================================================
//

public sealed class RelayCommand<T> : ICommand
{

    #region Declarations

    private readonly Predicate<T> _canExecuteMethod;
    private readonly Action<T> _executeMethod;

    #endregion

    #region Events

    public event EventHandler CanExecuteChanged
    {
        add
        {
            if (_canExecuteMethod != null)
            {
                CommandManager.RequerySuggested += value;
            }
        }
        remove
        {
            if (_canExecuteMethod != null)
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }

    #endregion

    #region Constructors

    public RelayCommand(Action<T> executeMethod)
        : this(executeMethod, null)
    {
    }

    public RelayCommand(Action<T> executeMethod, Predicate<T> canExecuteMethod)
    {

        if (executeMethod == null)
        {
            throw new ArgumentNullException("executeMethod", "Delegate comamnds can not be null");
        }

        _executeMethod = executeMethod;
        _canExecuteMethod = canExecuteMethod;
    }

    #endregion

    #region Methods

    public bool CanExecute(object parameter)
    {
        if (_canExecuteMethod == null)
        {
            return true;
        }

        return _canExecuteMethod((T)parameter);
    }

    public void Execute(object parameter)
    {
        _executeMethod((T)parameter);
    }
}

    #endregion


