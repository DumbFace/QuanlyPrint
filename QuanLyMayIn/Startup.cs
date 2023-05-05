using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(QuanLyMayIn.Startup))]
namespace QuanLyMayIn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            app.MapSignalR();
        }

        
    }
}