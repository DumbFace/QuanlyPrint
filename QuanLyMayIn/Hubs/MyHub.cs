using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayIn.Hubs
{
    [HubName("data_Hub")]
    public class MyHub : Hub 
    {
        public static void ReloadData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.Update();
        }
    }
}