using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SignalR.ViewModels
{
    class ChatVM : NotifyPropertyChangedBase
    {
        private int _LoginCount;
        public int LoginCount
        {
            get { return _LoginCount; }
            set
            {
                _LoginCount = value;
                notifyChange();
            }
        }


        public ObservableCollection<BaseHub.LogonData> Logins { get; set; } = new ObservableCollection<BaseHub.LogonData>();


        public ObservableCollection<BaseHub.ChatMessageData> Chats { get; set; } = new ObservableCollection<BaseHub.ChatMessageData>();


        private string _ChatMessage;
        public string ChatMessage
        {
            get { return _ChatMessage; }
            set
            {
                _ChatMessage = value;
                notifyChange();
            }
        }



    }
}
