using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class NetIO 
{



    private static NetIO instance;
    static NetIO()
    {
        instance = new NetIO();
    }
    /// <summary>
    /// 单例模式
    /// </summary>
    public static NetIO Instance
    {
        get
        {
            return instance;
        }
    }

    private Socket client;
    private string ip = "127.0.0.1";
    private int port = 55509;

    private byte[] readBuff = new byte[1024];

    List<byte> cache = new List<byte>();
    private bool isReading = false;


    List<SocketModel> messages = new List<SocketModel>();

    private NetIO()
    {
        try
        {
            // 创建客户端连接对象
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 连接远程服务器
            client.Connect(ip, port);
            // 异步接收消息，消息到达后，会直接写入到 缓冲区 readBuff
            client.BeginReceive(readBuff, 0, 1024, SocketFlags.None,
                Receive_Callback, client);

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }



    /// <summary>
    /// 收到消息回调
    /// </summary>
    /// <param name="ar"></param>
    private void Receive_Callback(IAsyncResult ar)
    {
        try
        {
            // 获取当前收到消息的长度
            int length = client.EndReceive(ar);
            byte[] message = new byte[length];
            Buffer.BlockCopy(readBuff, 0, message, 0, length);
            cache.AddRange(message);

            if (!isReading)
            {
                isReading = true;
                onData();
            }

            // 尾递归； 异步接收消息，消息到达后，会直接写入到 缓冲区 readBuff
            client.BeginReceive(readBuff, 0, 1024, SocketFlags.None,
                Receive_Callback, client);

        }
        catch (Exception ex)
        {
            Debug.Log("远程服务器主动断开连接");
            client.Close();
        }
    }


    public void write(byte type,int area,int command,object message)
    {
        ByteArray ba = new ByteArray();
        ba.write(type);
        ba.write(area);
        ba.write(command);
        //判断消息体是否为空  不为空则序列化后写入
        if (message != null)
        {
            ba.write(SerializeUtil.encode(message));
        
        }
        // 粘包处理
        ByteArray arr1 = new ByteArray();
        // 长度编码
        arr1.write(ba.Length);
        // 消息体编码
        arr1.write(ba.getBuff());
        try
        {
            client.Send(arr1.getBuff());
        }
        catch (Exception ex)
        {
            Debug.Log($"断开连接 ${ex.Message}");
        }

    }


    //缓存中有数据处理
    void onData()
    {
        //长度解码返回空，说明消息体不全，等待下条消息补全
        byte[] result = decode(ref cache);

        if(result == null)
        {
            isReading = false;
            return;
        }

        SocketModel message = mdecode(result);
        if(message == null)
        {
            isReading = false;
            return;
        }
        // 进行消息的处理
        messages.Add(message);

        //尾递归 防止在消息处理过程中 有其他消息到达而没有经过处理
        onData();
    }

    /// <summary>
    /// 粘包长度解码
    /// </summary>
    /// <param name="cache"></param>
    /// <returns></returns>
    public static byte[] decode(ref List<byte> cache)
    {
        if (cache.Count < 4) return null;

        MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并将缓存数据写入进去
        BinaryReader br = new BinaryReader(ms);//二进制读取流
        int length = br.ReadInt32();//从缓存中读取int型消息体长度
                                    //如果消息体长度 大于缓存中数据长度 说明消息没有读取完 等待下次消息到达后再次处理
        if (length > ms.Length - ms.Position)
        {
            return null;
        }
        //读取正确长度的数据
        byte[] result = br.ReadBytes(length);
        //清空缓存
        cache.Clear();
        //将读取后的剩余数据写入缓存
        cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
        br.Close();
        ms.Close();
        return result;
    }


    /// <summary>
    /// 消息体反序列化
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SocketModel mdecode(byte[] value)
    {
        ByteArray ba = new ByteArray(value);
        SocketModel model = new SocketModel();
        byte type;
        int area;
        int command;
        //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
        ba.read(out type);
        ba.read(out area);
        ba.read(out command);
        model.type = type;
        model.area = area;
        model.command = command;
        //判断读取完协议后 是否还有数据需要读取 是则说明有消息体 进行消息体读取
        if (ba.Readnable)
        {
            byte[] message;
            //将剩余数据全部读取出来
            ba.read(out message, ba.Length - ba.Position);
            //反序列化剩余数据为消息体
            model.message = SerializeUtil.decode(message);
        }
        ba.Close();
        return model;
    }

}
