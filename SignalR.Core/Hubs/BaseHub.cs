using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Transports;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SignalR.Core.Hubs
{
    [HubName("baseHub")]
    //[Authorize]
    public class BaseHub : Hub
    {
        // Client IP-address:
        // Context.Request.Environment["server.RemoteIpAddress"]
        // - se også: http://stackoverflow.com/questions/11044361/signalr-get-caller-ip-address

        static int _hitCount;

        public void RecordHit()
        {
            //_hitCount += 1;
            //base.Clients.All.onHitCountUpdated(_hitCount);

        }
        [HubMethodName("onChat")]
        public void OnChat(string chatMessage)
        {
            var username = string.IsNullOrEmpty(Context.User.Identity.Name) ? $"[{Context.ConnectionId}]" : Context.User.Identity.Name;
            base.Clients.All.onChat(username, chatMessage);
        }

        public override Task OnConnected()
        {
            // Deliver connections, prior to this
            var heartBeat = GlobalHost.DependencyResolver.Resolve<ITransportHeartbeat>();
            foreach (var connection in heartBeat.GetConnections())
            {
                if (connection.ConnectionId!=Context.ConnectionId)
                    Clients.Caller.onLogin(connection.ConnectionId, "previous logins", "");
            }

            _hitCount++;
            fireHitCountUpdated(_hitCount);
            var username = string.IsNullOrEmpty(Context.User.Identity.Name) ? $"[{Context.ConnectionId}]" : Context.User.Identity.Name;
            var ipAddress = Context.Request.Environment["server.RemoteIpAddress"];
            Clients.All.onLogin(Context.ConnectionId, username, ipAddress);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //Groups.Remove(Context.ConnectionId, "All");
            _hitCount--;
            fireHitCountUpdated(_hitCount);
            Clients.All.onLogoff(Context.ConnectionId);
           return base.OnDisconnected(stopCalled);
        }

        private void fireHitCountUpdated(int count)
        {
            Clients.All.onHitCountUpdated(count);
        }
    }
}