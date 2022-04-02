using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Network
{
    public class ConnectionHandler : IDisposable, IConsumer<byte[]>
    {
        private bool _isListening;

        private readonly List<Connection> _connectionPool = new List<Connection>();

        private TcpListener _tcpListener;

        public delegate void HandleMessage(string message);
        
        public event HandleMessage onMessageConsume;
        
        public delegate void HandleConnectedPlayer(string address);
        
        public event HandleConnectedPlayer onPlayerConnected;

        public void Consume(byte[] message)
        {
            onMessageConsume?.Invoke(Encoding.UTF8.GetString(message));
        }
        
        public void Host(int port)
        {
            _tcpListener = TcpListener.Create(port);
            _tcpListener.Start();
            _isListening = true;
            new Thread(AcceptConnection).Start();
        }

        private void AcceptConnection()
        {
            while (_isListening)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                Debug.Log("Accepting tcpClient...");
                try
                {
                    Connection connection = new Connection(client, this);
                    Debug.Log("Connection created: " + connection);
                    _connectionPool.Add(connection);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                onPlayerConnected?.Invoke(((IPEndPoint) client.Client.RemoteEndPoint).Address + ":" + ((IPEndPoint) client.Client.RemoteEndPoint).Port);
            }
        }

        public void Connect(string address, int port)
        {
            Debug.Log("Connecting...");
            TcpClient tcpClient = new TcpClient(address, port);
            if (!tcpClient.Connected) throw new Exception("Could not connect");
            Debug.Log("Connected!");
            _connectionPool.Add(new Connection(tcpClient, this));
        }

        public void Send(byte[] bytes)
        {
            if (_connectionPool.Count == 0) throw new Exception("No available connections");
            foreach (Connection connection in _connectionPool)
            {
                connection.Send(bytes);
            }
        }

        public void Dispose()
        {
            Debug.Log("Connection Handler is disposing...");
            _isListening = false;
            _tcpListener?.Stop();
            foreach (Connection connection in _connectionPool)
            {
                connection.Dispose();
            }
        }
    }
}