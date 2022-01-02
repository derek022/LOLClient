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
    /// 登录返回处理
    /// </summary>
    /// <param name="value"></param>
    private void loginCallback(int value)
    {
        SendMessage("openLoginBtn");

        switch(value)
        {
            case 0:
                // 加载游戏主场景
                SceneManager.LoadScene(1);
                
                break;

            case -1:
                WarmingManager.errors.Add(new WarningModel("账号不存在"));
                break;

            case -2:
                WarmingManager.errors.Add(new WarningModel("账号在线"));
                break;

            case -3:
                WarmingManager.errors.Add(new WarningModel("密码错误"));
                break;

            case -4:
                WarmingManager.errors.Add(new WarningModel("输入不合法"));
                break;
        }
    }

    /// <summary>
    /// 注册返回处理
    /// </summary>
    /// <param name="value"></param>
    private void regCallback(int value)
    {
        switch (value)
        {
            case 0:
                // 注册成功
                break;

            case -1:
                WarmingManager.errors.Add(new WarningModel("账号重复"));
                break;

            case -2:
                WarmingManager.errors.Add(new WarningModel("账号不合法"));
                break;

            case -3:
                WarmingManager.errors.Add(new WarningModel("密码不合法"));
                break;
        }
    }

}
