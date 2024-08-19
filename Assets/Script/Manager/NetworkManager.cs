using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager
{

    public void Init()
    {
        Connector connector = new Connector();
        IPAddress ipAdress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAdress, 4000);

        connector.Connect(endPoint, () => { return new ServerSession(); });
    }
}
