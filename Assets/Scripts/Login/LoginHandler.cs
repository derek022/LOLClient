using GameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour,IHandler
{
    public void MessageReceive(SocketModel model)
    {
        
        switch(model.command)
        {
            case LoginProtocol.LOGIN_SRES:
                loginCallback(model.GetMessage<int>());
                break;

            case LoginProtocol.REG_SRES:
                regCallback(model.GetMessage<int>());
                break;

        }
    }

    /// <summary>
    /// ��¼���ش���
    /// </summary>
    /// <param name="value"></param>
    private void loginCallback(int value)
    {
        SendMessage("openLoginBtn");

        switch(value)
        {
            case 0:
                // ������Ϸ������
                SceneManager.LoadScene(1);
                
                break;

            case -1:
                WarmingManager.errors.Add(new WarningModel("�˺Ų�����"));
                break;

            case -2:
                WarmingManager.errors.Add(new WarningModel("�˺�����"));
                break;

            case -3:
                WarmingManager.errors.Add(new WarningModel("�������"));
                break;

            case -4:
                WarmingManager.errors.Add(new WarningModel("���벻�Ϸ�"));
                break;
        }
    }

    /// <summary>
    /// ע�᷵�ش���
    /// </summary>
    /// <param name="value"></param>
    private void regCallback(int value)
    {
        switch (value)
        {
            case 0:
                // ע��ɹ�
                break;

            case -1:
                WarmingManager.errors.Add(new WarningModel("�˺��ظ�"));
                break;

            case -2:
                WarmingManager.errors.Add(new WarningModel("�˺Ų��Ϸ�"));
                break;

            case -3:
                WarmingManager.errors.Add(new WarningModel("���벻�Ϸ�"));
                break;
        }
    }

}
