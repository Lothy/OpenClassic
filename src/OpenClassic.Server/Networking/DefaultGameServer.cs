﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OpenClassic.Server.Networking
{
    public class DefaultGameServer : IGameServer
    {
        private readonly SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
        private readonly EventHandler<SocketAsyncEventArgs> acceptCompleteEventHandler;
        private readonly EventHandler<SocketAsyncEventArgs> readWriteCompleteEventHandler;

        private readonly Socket acceptSocket;

        private Thread firstThread = null;

        public DefaultGameServer()
        {
            acceptCompleteEventHandler = new EventHandler<SocketAsyncEventArgs>(AcceptCompletion);
            acceptArgs.Completed += acceptCompleteEventHandler;

            readWriteCompleteEventHandler = new EventHandler<SocketAsyncEventArgs>(ReadWriteCompletion);

            var addr = IPAddress.Loopback;
            acceptSocket = Bind(new IPEndPoint(addr, 43594));

            Console.WriteLine("Socket bound");

            AcceptStart();

            Console.ReadKey();
        }

        private Socket Bind(IPEndPoint endpoint)
        {
            Socket s = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                s.LingerState.Enabled = false;
                s.ExclusiveAddressUse = false;
                s.Bind(endpoint);
                s.Listen(8);

                return s;
            }
            catch (Exception e)
            {
                if (e is SocketException)
                {
                    SocketException se = (SocketException)e;

                    if (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        // WSAEADDRINUSE
                        Console.WriteLine("Listener Failed: {0}:{1} (In Use)", endpoint.Address, endpoint.Port);
                    }
                    else if (se.SocketErrorCode == SocketError.AddressNotAvailable)
                    {
                        // WSAEADDRNOTAVAIL
                        Console.WriteLine("Listener Failed: {0}:{1} (Unavailable)", endpoint.Address, endpoint.Port);
                    }
                    else
                    {
                        Console.WriteLine("Listener Exception:");
                        Console.WriteLine(e);
                    }
                }

                return null;
            }
        }

        private void AcceptStart()
        {
            var willRaiseEvent = false;
            var args = acceptArgs;

            do
            {
                try
                {
                    // AcceptAsync returns true when the IO operation is pending (and will be
                    // handled by a callback to args.Completed once finished).
                    // It returns false when the IO operation completed synchronously though,
                    // which means we need to synchronously invoke AcceptProcess ourselves.

                    willRaiseEvent = acceptSocket.AcceptAsync(args);
                }
                catch (SocketException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }

                if (!willRaiseEvent)
                {
                    AcceptProcess(args);
                }
            } while (!willRaiseEvent);
        }

        private void AcceptCompletion(object sender, SocketAsyncEventArgs e)
        {
            AcceptProcess(e);
            AcceptStart();
        }

        private void ReadWriteCompletion(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    break;
                case SocketAsyncOperation.Send:
                    break;
                default:
                    break;
            }
        }

        private void AcceptProcess(SocketAsyncEventArgs e)
        {
            var readWriteArgs = new SocketAsyncEventArgs();
            readWriteArgs.Completed += readWriteCompleteEventHandler;

            var willRaiseEvent = e.AcceptSocket.ReceiveAsync(readWriteArgs);
            if (!willRaiseEvent)
            {
                ReceiveProcess(readWriteArgs);
            }

            //if (e.SocketError == SocketError.Success)
            //{
            //    Enqueue(e.AcceptSocket);
            //}
            //else
            //{
            //    Release(e.AcceptSocket);
            //}

            e.AcceptSocket = null;
        }

        private void ReceiveProcess(SocketAsyncEventArgs e)
        {

        }

        private void SendProcess(SocketAsyncEventArgs e)
        {

        }

        private void Enqueue(Socket socket)
        {
            //lock (m_AcceptedSyncRoot)
            //{
            //    m_Accepted.Enqueue(socket);
            //}

            //Core.Set();
        }

        private void Release(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
            }

            //try
            //{
            //    socket.Close();
            //}
            //catch (SocketException ex)
            //{
            //}
        }
    }
}
