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
    /// ����ģʽ
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
            // �����ͻ������Ӷ���
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // ����Զ�̷�����
            client.Connect(ip, port);
            // �첽������Ϣ����Ϣ����󣬻�ֱ��д�뵽 ������ readBuff
            client.BeginReceive(readBuff, 0, 1024, SocketFlags.None,
                Receive_Callback, client);

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }



    /// <summary>
    /// �յ���Ϣ�ص�
    /// </summary>
    /// <param name="ar"></param>
    private void Receive_Callback(IAsyncResult ar)
    {
        try
        {
            // ��ȡ��ǰ�յ���Ϣ�ĳ���
            int length = client.EndReceive(ar);
            byte[] message = new byte[length];
            Buffer.BlockCopy(readBuff, 0, message, 0, length);
            cache.AddRange(message);

            if (!isReading)
            {
                isReading = true;
                onData();
            }

            // β�ݹ飻 �첽������Ϣ����Ϣ����󣬻�ֱ��д�뵽 ������ readBuff
            client.BeginReceive(readBuff, 0, 1024, SocketFlags.None,
                Receive_Callback, client);

        }
        catch (Exception ex)
        {
            Debug.Log("Զ�̷����������Ͽ�����");
            client.Close();
        }
    }


    public void write(byte type,int area,int command,object message)
    {
        ByteArray ba = new ByteArray();
        ba.write(type);
        ba.write(area);
        ba.write(command);
        //�ж���Ϣ���Ƿ�Ϊ��  ��Ϊ�������л���д��
        if (message != null)
        {
            ba.write(SerializeUtil.encode(message));
        
        }
        // ճ������
        ByteArray arr1 = new ByteArray();
        // ���ȱ���
        arr1.write(ba.Length);
        // ��Ϣ�����
        arr1.write(ba.getBuff());
        try
        {
            client.Send(arr1.getBuff());
        }
        catch (Exception ex)
        {
            Debug.Log($"�Ͽ����� ${ex.Message}");
        }

    }


    //�����������ݴ���
    void onData()
    {
        //���Ƚ��뷵�ؿգ�˵����Ϣ�岻ȫ���ȴ�������Ϣ��ȫ
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
        // ������Ϣ�Ĵ���
        messages.Add(message);

        //β�ݹ� ��ֹ����Ϣ��������� ��������Ϣ�����û�о�������
        onData();
    }

    /// <summary>
    /// ճ�����Ƚ���
    /// </summary>
    /// <param name="cache"></param>
    /// <returns></returns>
    public static byte[] decode(ref List<byte> cache)
    {
        if (cache.Count < 4) return null;

        MemoryStream ms = new MemoryStream(cache.ToArray());//�����ڴ������󣬲�����������д���ȥ
        BinaryReader br = new BinaryReader(ms);//�����ƶ�ȡ��
        int length = br.ReadInt32();//�ӻ����ж�ȡint����Ϣ�峤��
                                    //�����Ϣ�峤�� ���ڻ��������ݳ��� ˵����Ϣû�ж�ȡ�� �ȴ��´���Ϣ������ٴδ���
        if (length > ms.Length - ms.Position)
        {
            return null;
        }
        //��ȡ��ȷ���ȵ�����
        byte[] result = br.ReadBytes(length);
        //��ջ���
        cache.Clear();
        //����ȡ���ʣ������д�뻺��
        cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
        br.Close();
        ms.Close();
        return result;
    }


    /// <summary>
    /// ��Ϣ�巴���л�
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
        //�������ж�ȡ ����Э��  ��ȡ����˳������д��˳�򱣳�һ��
        ba.read(out type);
        ba.read(out area);
        ba.read(out command);
        model.type = type;
        model.area = area;
        model.command = command;
        //�ж϶�ȡ��Э��� �Ƿ���������Ҫ��ȡ ����˵������Ϣ�� ������Ϣ���ȡ
        if (ba.Readnable)
        {
            byte[] message;
            //��ʣ������ȫ����ȡ����
            ba.read(out message, ba.Length - ba.Position);
            //�����л�ʣ������Ϊ��Ϣ��
            model.message = SerializeUtil.decode(message);
        }
        ba.Close();
        return model;
    }

}
