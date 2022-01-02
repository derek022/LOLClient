using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MonoBehaviourEx
{

    /// <summary>
    /// 扩展MonoBehaviour 发送消息
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="type"></param>
    /// <param name="area"></param>
    /// <param name="command"></param>
    /// <param name="message"></param>
    public static void WriteMessage(this MonoBehaviour mono, byte type, int area, int command, object message)
    {
        NetIO.Instance.write(type, area, command, message);
    }
}

