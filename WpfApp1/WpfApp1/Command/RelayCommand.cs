using System;
using System.Windows.Input;

namespace WpfApp1.Command
{
    /// <summary>
    /// A command implementation that allows actions to be defined directly in the ViewModel.
    /// Implements the ICommand interface for use in the MVVM pattern.
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action< object > _executeAction;
        private readonly Func< object, bool > _canExecute;
        private readonly bool _canExecuteCache;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="executeAction">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">A function that determines whether the command can execute.</param>
        /// <param name="canExecuteCache">Indicates whether the CanExecute result should be cached.</param>

        public RelayCommand( Action< object > executeAction, Func< object, bool > canExecute, bool canExecuteCache )
        {
            _canExecute = canExecute;
            _executeAction = executeAction;
            _canExecuteCache = canExecuteCache;
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute( object parameter )
        {
            if ( _canExecute == null )
            {
                return true;

            }
            else
            {
                return _canExecute( parameter );
            }
        }

        /// <summary>
        /// Occurs when changes affecting whether the command should execute happen.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public void Execute( object parameter )
        {
            _executeAction( parameter );
        }
    }
}
