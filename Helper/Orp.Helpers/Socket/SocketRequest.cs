using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZYSocket.share;

namespace Models.SocketModels
{
    public enum SocketRequestType
    {
        发起抢单 = 100,
        获取微信全局令牌 = 200
    }

    public class SocketRequestData
    {
        public SocketRequestType Type { get; set; }

        public object Data { get; set; }

        public override string ToString()
        {
            return string.Format("#{0}#{1}#", (int)this.Type, JsonConvert.SerializeObject(this.Data));
        }
    }
}
