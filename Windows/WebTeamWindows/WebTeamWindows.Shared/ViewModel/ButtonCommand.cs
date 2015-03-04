using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WebTeamWindows.ViewModel
{
    public class ButtonCommand : ICommand
    {
        private Action WhattoExecute;
        private Func<bool> WhentoExecute;
        public ButtonCommand(Action What, Func<bool> When) 
        {
            WhattoExecute = What;
            WhentoExecute = When;
        }
        public bool CanExecute(object parameter)
        {
            return WhentoExecute(); 
        }
        public void Execute(object parameter)
        {
            WhattoExecute();
        }

        public event EventHandler CanExecuteChanged;
    }
}
