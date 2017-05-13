using Models.SocketModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace System
{
    public class SocketHelper
    {
        public static SocketResponseData StartOrderSnapUp(Guid orderId)
        {
            string data = new SocketRequestData { Type = SocketRequestType.发起抢单, Data = orderId }.ToString();
            byte[] sendBytes = Encoding.UTF8.GetBytes(data);
            byte[] recvBytes = new byte[1024];
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ConfigHelper.Host), ConfigHelper.SocketPort));
                socket.Send(sendBytes, sendBytes.Length, SocketFlags.None);
                int recvLength = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                string receive = Encoding.UTF8.GetString(recvBytes, 0, recvLength);
                return JsonConvert.DeserializeObject<SocketResponseData>(receive);
            }
            catch (Exception ex)
            {
                return new SocketResponseData(SocketResponseType.错误, ex.ToString());
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
                socket.Dispose();
            }
        }

        /// <summary>
        /// 获取服务号全局令牌
        /// </summary>
        /// <returns></returns>
        public static string GetWxAccessToken()
        {
            string data = new SocketRequestData { Type = SocketRequestType.获取微信全局令牌, Data = null }.ToString();
            byte[] sendBytes = Encoding.UTF8.GetBytes(data);
            byte[] recvBytes = new byte[1024];
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ConfigHelper.Host), ConfigHelper.SocketPort));
                socket.Send(sendBytes, sendBytes.Length, SocketFlags.None);
                int recvLength = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                return Encoding.UTF8.GetString(recvBytes, 0, recvLength);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
                socket.Dispose();
            }
        }

        /// <summary>
        /// 获取App应用全局令牌
        /// </summary>
        /// <returns></returns>
        public static string GetWxAccessToken2()
        {
            string data = new SocketRequestData { Type = SocketRequestType.获取微信全局令牌, Data ="app" }.ToString();
            byte[] sendBytes = Encoding.UTF8.GetBytes(data);
            byte[] recvBytes = new byte[1024];
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ConfigHelper.Host), ConfigHelper.SocketPort));
                socket.Send(sendBytes, sendBytes.Length, SocketFlags.None);
                int recvLength = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                return Encoding.UTF8.GetString(recvBytes, 0, recvLength);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
                socket.Dispose();
            }
        }
    }
}