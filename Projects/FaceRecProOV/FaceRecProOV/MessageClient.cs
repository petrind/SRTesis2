using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RedCorona.Net;
using System.Net;
using System.Net.Sockets;

namespace MultiFaceRec
{
    /// <summary>
    /// http://www.codeproject.com/Articles/12286/Simple-Client-server-Interactions-using-C
    /// </summary>
    class MessageClient
    {
        bool mcConnect = false;
        public delegate void ImageRetrievedHandler(object sender, EventArgs e);
        //public event ImageRetrievedHandler ImageRetrieved;
        //void OnImageRetrieved(EventArgs e)
        //{
        //    if (ImageRetrieved != null)
        //        ImageRetrieved(this, e);
        //}

        byte[] bu;
        byte[] bl;
        public bool updatedUpper = false, updatedLower = false;

        ClientInfo client;

        public void Start()
        {
            Socket sock = Sockets.CreateTCPSocket("localhost", 2345);
            client = new ClientInfo(sock, false); // Don't start receiving yet
            client.MessageType = MessageType.CodeAndLength;
            client.OnReadMessage += new ConnectionReadMessage(ReadMessage);
            client.BeginReceive();
            mcConnect = true;
        }


        void ReadMessage(ClientInfo ci, uint code, byte[] buf, int len)
        {
            if (code == ClientInfo.ImageCodeUpper)
            {
                Console.WriteLine("Message length: " + len + ", code " + code.ToString("X8") + ", content:");
                bu = new byte[len];
                Array.Copy(buf, bu, len);
                //Console.WriteLine("  " + ByteBuilder.FormatParameter(new Parameter(ba, ParameterType.Byte)));
                updatedUpper = true;
                //OnImageRetrieved(EventArgs.Empty);
            }
            if (code == ClientInfo.ImageCodeLower)
            {
                Console.WriteLine("Message length: " + len + ", code " + code.ToString("X8") + ", content:");
                bl = new byte[len];
                Array.Copy(buf, bl, len);
                //Console.WriteLine("  " + ByteBuilder.FormatParameter(new Parameter(ba, ParameterType.Byte)));
                updatedLower = true;
                //OnImageRetrieved(EventArgs.Empty);
            }
        }
        public void sendCommand(string command)
        {
            client.SendMessage(ClientInfo.CommandCode, Encoding.UTF8.GetBytes(command));
        }
        public void sendMoveTo(float x, float y,float theta)
        {
            client.SendMessage(ClientInfo.moveToCode, Encoding.UTF8.GetBytes(x+","+y+","+theta));
        }
        public void sendVoice(string command)
        {
            client.SendMessage(ClientInfo.VoiceCode, Encoding.UTF8.GetBytes(command));
        }
        public bool isconnect()
        {
            return mcConnect;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">0=upper image, 1=lower image</param>
        /// <returns></returns>
        public byte[] getByte(int i)
        {
            switch (i)
            {
                case 0:
                    updatedUpper = false;
                    return bu;
                case 1:
                    updatedLower = false;
                    return bl;
                default:
                    return null;
            }
        }
    }
}
