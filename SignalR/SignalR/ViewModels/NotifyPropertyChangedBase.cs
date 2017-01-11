using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SignalR.ViewModels
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void notifyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
