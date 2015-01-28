using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Assets.Slots.Scripts.Core.Server;
using Assets.Slots.Scripts.Core.Server.Facade;
using Assets.Slots.Scripts.Core.Server.Http;
using Assets.Slots.Scripts.Core.Server.Model;
using Assets.Slots.Scripts.Core.Server.Service;

namespace SlotsServerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            ClientSessionData.Instance.IsRegistered = true;
            ClientSessionData.Instance.ServerType = ServerType.TestFun;
            ClientSessionData.Instance.UserAgentHeaderVal = "android_client;com.vegaslot-mobi-fun;2.8.0 (200086)";
            ClientSessionData.Instance.XProtocol = "vegaslot";
            ClientSessionData.Instance.Login = "achernyy@mail.ru";
            ClientSessionData.Instance.Email = "achernyy@mail.ru";
            ClientSessionData.Instance.Password = "achernyy";
            ClientSessionData.Instance.TrValue = "";

            new AccountFacade().LoginOrRegister();
            Console.ReadLine();
        }
    }
}
