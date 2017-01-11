using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNet.SignalR.Client;

namespace SignalR.Hubs
{
    public class BaseHub
    {
        #region Singleton instance

        private static BaseHub _Instance = null;
        public static BaseHub Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BaseHub();
                return _Instance;
            }
        }

        #endregion


        private BaseHub()
        { }

        IHubProxy _BaseHubProxy;
        /// <summary>
        /// Establish connection to the BaseHub
        /// </summary>
        public async void SetupConnection()
        {
            var hubConnection = new HubConnection(Settings.Instance.BaseHubUrl);
            _BaseHubProxy = hubConnection.CreateHubProxy("baseHub");


            // NOTE!
            // Events are fired on MainThread, so Clients do not have to handle this!

            // onLogin:
            _BaseHubProxy.On<string,string, string>("onLogin", (connectionId, userName, ipAddress ) =>
            {
                if (OnHubLogon != null)
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        OnHubLogon(this, new LogonData { ConnectionId = connectionId, UserName = userName, IpAddress = ipAddress });
                    });
            });

            // onLogoff
            _BaseHubProxy.On<string>("onLogoff", (connectionId) =>
            {
                if (OnHubLogoff!=null)
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    OnHubLogoff(this, new LogoffData { ConnectionId = connectionId });
                });
            });

            // onHitCountUpdated
            _BaseHubProxy.On<int>("onHitCountUpdated", (hitCount) =>
            {
                if (OnLogonCount!=null)
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    OnLogonCount(this, new LogonCountData { Count = hitCount });
                });
            });

            // onChat
            _BaseHubProxy.On<string, string>("onChat", (userName, message) =>
            {
                if (OnHubChatMessage!=null)
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    OnHubChatMessage(this, new ChatMessageData { UserName = userName, Message = message });
                });
            });

            await hubConnection.Start();

        }

        #region DownStream ( Server --> Client )

        public event EventHandler<ChatMessageData> OnHubChatMessage;
        public class ChatMessageData
        {
            public string UserName { get; set; }
            public string Message { get; set; }
        }

        public event EventHandler<LogonData> OnHubLogon;
        public class LogonData
        {
            public string ConnectionId { get; set; }
            public string UserName { get; set; }
            public string IpAddress { get; set; }

            public string UserAndIp => $"{UserName}/{IpAddress}"; 
        }

        public event EventHandler<LogoffData> OnHubLogoff;
        public class LogoffData
        {
            public string ConnectionId { get; set; }
        }

        public event EventHandler<LogonCountData> OnLogonCount;
        public class LogonCountData
        {
            public int Count { get; set; }
        }

        #endregion

        #region UpStream ( Client --> Server )

        public async void SendChat(string chatMessage)
        {
            await _BaseHubProxy.Invoke("onChat", chatMessage);
        }
        #endregion
    }
}
