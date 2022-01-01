using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameProtocol;


/// <summary>
/// ��Ϣת��
/// </summary>
public class NetMessageUtil : MonoBehaviour,IHandler
{

    private LoginHandler loginHandler;

    void Start()
    {
        loginHandler = GetComponent<LoginHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(NetIO.Instance.messages.Count>0)
        {
            SocketModel model = NetIO.Instance.messages[0];

            StartCoroutine("MessageReceive", model);

            NetIO.Instance.messages.RemoveAt(0);
        }
    }


    public void MessageReceive(SocketModel model)
    {
        switch(model.type)
        {
            case Protocol.TYPE_LOGIN:
                loginHandler.MessageReceive(model);
                break;
        }
    }
}
