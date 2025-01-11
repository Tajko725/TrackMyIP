using System.Windows.Input;

namespace TrackMyIP.Models
{
    /// <summary>
    /// A flexible implementation of the <see cref="ICommand"/> interface.
    /// Allows for binding commands with custom execution and condition logic.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with execution and condition logic.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">A function to determine whether the command can be executed. Defaults to always executable if null.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="execute"/> parameter is null.</exception>
        public RelayCommand(Action<object?> execute, Func<object?, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with only execution logic.
        /// The command will always be executable.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        public RelayCommand(Action<object?> execute)
            : this(execute, null!)
        {
        }

        /// <summary>
        /// Occurs when changes affecting whether or not the command should execute are detected.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The command parameter to pass to the condition logic.</param>
        /// <returns>
        /// True if the command can execute, or if no condition logic is provided;
        /// otherwise, false.
        /// </returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command's logic.
        /// </summary>
        /// <param name="parameter">The command parameter to pass to the execution logic.</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}