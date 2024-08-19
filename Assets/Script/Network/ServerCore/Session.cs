using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;

        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int numOfProcess = 0;

            while (true)
            {
                if (buffer.Count < HeaderSize)
                    break;

                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                    break;

                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                numOfProcess += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            return numOfProcess;
        }

        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }
    public abstract class Session
    {
        public static readonly int CapacitySize = 1000;
        object _lock = new object();
        Socket _socket;

        SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();
        BufferManager _buffManager = new BufferManager(1024 * 10);

        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>(CapacitySize);
        List<ArraySegment<byte>> _sendList = new List<ArraySegment<byte>>(CapacitySize);

        public abstract void OnConnected();
        public abstract int OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend();
        public abstract void OnDisconnected();

        public void Start(Socket socket)
        {
            _socket = socket;

            _receiveArgs.Completed += CompleteReceive;
            _sendArgs.Completed += CompleteSend;

            ProcessReceive();
        }

        public void Send(ArraySegment<byte> segment)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(segment);
                if (_sendList.Count == 0)
                    ProcessSend();
            }
        }

        private void Clear()
        {
            lock (_lock)
            {
                _sendQueue.Clear();
                _sendList.Clear();
                _socket?.Dispose();
            }
        }

        private void ProcessSend()
        {
            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> segment = _sendQueue.Dequeue();
                _sendList.Add(segment);
            }
            _sendArgs.BufferList = _sendList;

            bool willRaiseEvent = _socket.SendAsync(_sendArgs);
            if (willRaiseEvent == false)
                CompleteSend(null, _sendArgs);
        }

        private void CompleteSend(object sneder, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    _sendArgs.BufferList = null;
                    _sendList.Clear();

                    OnSend();

                    if (_sendQueue.Count > 0)
                        ProcessSend();
                }
                else
                {
                    CloseClientSocket();
                }
            }
        }

        private void ProcessReceive()
        {
            _buffManager.Refresh();
            ArraySegment<byte> segment = _buffManager.RentBuffer();
            _receiveArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);


            bool willRaiseEvent = _socket.ReceiveAsync(_receiveArgs);
            if (willRaiseEvent == false)
            {
                CompleteReceive(null, _receiveArgs);
            }
        }

        private void CompleteReceive(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                if (_buffManager.CheckAndWrite(args.BytesTransferred) == false)
                {
                    CloseClientSocket();
                    return;
                }

                //클라에서 100개를 보냈는데 아직 80개 밖에 안왔다면 80개 안에서 패킷 조랍할 수 있는 것들은 조립하고 
                //몇byte 조립했는지를 받아서 조립하지 못한 패킷을 챙겨주기 위해 readPos를 둔다
                //다시 receive가 들어올 때 중간에 잘린 패킷은 결국엔 readPos부터 읽기 때문에 챙겨줄 수 있음
                int numOfProcess = OnRecv(_buffManager.GetSegmentFromReadToWrite());
                if (numOfProcess < 0 || numOfProcess > _buffManager.SizeOfReadToWrite)
                {
                    CloseClientSocket();
                    return;
                }
                else
                {
                    if (_buffManager.CheckAndRead(numOfProcess) == false)
                    {
                        CloseClientSocket();
                        return;
                    }
                }

                ProcessReceive();
            }
            else
            {
                CloseClientSocket();
            }
        }

        private void CloseClientSocket()
        {
            OnDisconnected();
            // close the socket associated with the client
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            // throws if client process has already closed
            catch (Exception e) { Console.WriteLine(e); }
            _socket.Close();
            Clear();

        }
    }
}
