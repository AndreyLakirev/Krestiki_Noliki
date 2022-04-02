using System;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

namespace Network
{
    public class Connection : IDisposable
    {
        private bool isListening;
        
        private readonly TcpClient _tcpClient;

        private readonly IConsumer<byte[]> _consumer;

        private Thread _listener;
        
        //In milliseconds
        private int _refreshRate = 1000;
        public int refreshRate
        {
            get => _refreshRate;
            set { if (value > 0) _refreshRate = value; }
        }
        
        public Connection([NotNull] TcpClient tcpClient, [NotNull] IConsumer<byte[]> consumer)
        {
            _tcpClient = tcpClient;
            _consumer = consumer;
            isListening = true;
            new Thread(Listen).Start();
        }
        
        public void Send(byte[] bytes)
        {
            NetworkStream stream = _tcpClient.GetStream();
            {
                stream.Write(bytes, 0, bytes.Length);
                Debug.Log("Message sent");
            }
        }

        private void Listen()
        {
            {
                while (isListening)
                {
                    Debug.Log("Listening...");
                    NetworkStream stream = _tcpClient.GetStream();
                        if (stream.DataAvailable)
                        {
                            byte[] bytes = new byte[1024];
                            stream.Read(bytes, 0, bytes.Length);
                            _consumer.Consume(bytes);
                            Debug.Log("Message consumed");
                        }
                    Thread.Sleep(_refreshRate);
                }
            }
        }

        public void Dispose()
        {
            isListening = false;
            _tcpClient.Dispose();
        }
    }
}