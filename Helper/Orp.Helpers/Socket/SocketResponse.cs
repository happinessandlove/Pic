using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZYSocket.share;

namespace Models.SocketModels
{
    public enum SocketResponseType
    {
        成功 = 200,

        错误 = 500,
        请求数据格式错误=501,
        订单号不合法=502,
        订单不存在=503,
        数据库更新错误=504,

        没有工人抢单=601,
    }

    public class SocketResponseData
    {
        public SocketResponseData()
        { }

        public SocketResponseData(SocketResponseType type)
        {
            this.Type = type;
        }
        public SocketResponseData(SocketResponseType type,string data)
        {
            this.Type = type;
            this.Data = data;
        }

        public SocketResponseType Type { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString());
        }

    }
}
