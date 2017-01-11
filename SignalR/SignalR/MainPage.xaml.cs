using SignalR.Hubs;
using SignalR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SignalR
{
    public partial class MainPage : ContentPage
    {
        private ChatVM model = new ChatVM();

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = model;

            // NOTE: These events are dispatched to the UI thread, in the Hub implementation
            BaseHub.Instance.OnLogonCount += (s, e) =>
            {
                model.LoginCount = e.Count;
            };
            BaseHub.Instance.OnHubLogon += (s, e) =>
            {
                model.Logins.Add(e);
            };
            BaseHub.Instance.OnHubLogoff += (s, e) =>
            {
                foreach (var login in model.Logins)
                {
                    if (login.ConnectionId == e.ConnectionId)
                    {
                        model.Logins.Remove(login);
                        break;
                    }
                }
            };
            BaseHub.Instance.OnHubChatMessage += (s, e) =>
            {
                if (e != null)
                    model.Chats.Add(e);
            };
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            BaseHub.Instance.SendChat(model.ChatMessage);
            model.ChatMessage = "";
        }
        private void ConnectButton_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            BaseHub.Instance.SetupConnection();
        }
    }
}
