using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerCore
{
    public class Connector
    {

        Func<Session> _session;
        public Action OnConnectedAction { get; set; }
        public Action OnFailedAction { get; set; }

        public void Connect(IPEndPoint endPoint, Func<Session> session)
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _session = session;

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(CompleteConnect);
            args.RemoteEndPoint = endPoint;
            args.UserToken = socket;
            StartConnect(args);
        }

        private void StartConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;
            if (socket == null)
                return;
            bool willRaiseEvent = false;
            willRaiseEvent = socket.ConnectAsync(args);

            if (willRaiseEvent == false)
            {
                CompleteConnect(null, args);
            }
        }

        private void CompleteConnect(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _session.Invoke();
                session.Start(args.ConnectSocket);
                session.OnConnected();
                CallbackQueue.Instance.Push(OnConnectedAction);
            }
            else
            {
                Debug.Log("Failed Connect");
                CallbackQueue.Instance.Push(OnFailedAction);
                StartConnect(args);
            }
        }
    }
}
