using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MediaWikiPublisher.UI
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> handler;
        private bool isEnabled;

        public RelayCommand(Action<object> handler, bool isEnabled = true)
        {
            this.handler = handler;
            this.isEnabled = isEnabled;
        }

        public RelayCommand(Action handler, bool isEnabled = true)
            : this(_=>handler(), isEnabled)
        { }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                NotifyCanExecuteChanged();
            }
        }

        #region Implementation of ICommand

        /// <summary>
        /// Определяет метод, вызываемый при вызове данной команды.
        /// </summary>
        /// <param name="parameter">Данные, используемые данной командой.Если данная команда не требует передачи данных, этот объект может быть установлен в null.</param>
        public void Execute(object parameter)
        {
            handler(parameter);
        }

        /// <summary>
        /// Определяет метод, который определяет, может ли данная команда выполняться в ее текущем состоянии.
        /// </summary>
        /// <returns>
        /// true, если команда может быть выполнена; в противном случае — false.
        /// </returns>
        /// <param name="parameter">Данные, используемые данной командой.Если данная команда не требует передачи данных, этот объект может быть установлен в null.</param>
        public bool CanExecute(object parameter)
        {
            return isEnabled;
        }

        public event EventHandler CanExecuteChanged;

        private void NotifyCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
