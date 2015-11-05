using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_Client.Source
{
    class ServerManager
    {
        public static Tuple<string, string> GetServer()
        {
            WebClient client = new WebClient();
            string response = client.DownloadString("http://m.agar.io");
            var split = response.Split('\n');
            return new Tuple<string, string>(split[0], split[1]);
        }
    }
}
