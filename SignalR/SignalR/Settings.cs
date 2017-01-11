using System;
using System.Collections.Generic;
using System.Text;

namespace SignalR
{
    // TODO Needs to be persisted (Currently hardcoded!)
    public class Settings
    {
        #region Singleton instance

        private static Settings _Instance = new Settings();

        public static Settings Instance => _Instance;
        private Settings()
        {

        }

        #endregion

        public string BaseHubUrl => "http://localhost:30450/";
    }
}
