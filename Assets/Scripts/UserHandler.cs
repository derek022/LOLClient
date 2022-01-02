using GameProtocol;
using GameProtocol.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UserHandler :MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case UserProtocol.CREATE_SRES:
                create(model.GetMessage<bool>());
                break;

            case UserProtocol.INFO_SRES:
                info(model.GetMessage<UserDTO>());
                break;

            case UserProtocol.ONLINE_SRES:
                online(model.GetMessage<UserDTO>());
                break;
        }
    }

    private void info(UserDTO user)
    {
        // 是否有角色
        if(user == null)
        {
            // 显示创建角色面板
            SendMessage("OpenCreate");
        }
        else
        {
            // 开启登录流程
            SendMessage("CloseCreate");
            this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.ONLINE_CREQ, null);

        }
    }


    private void online(UserDTO user)
    {
        GameData.user = user;
        // 登录之后，遮罩移除
        SendMessage("closeMask");
        // 刷新UI数据
        SendMessage("RefershView");

    }


    private void create(bool value)
    {
        if(value)
        {
            WarmingManager.errors.Add(new WarningModel("创建成功", () =>
            {
                this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.ONLINE_CREQ, null);
            }));

            // 关闭创建面板
            SendMessage("CloseCreate");
        }
        else
        {
            // 刷新创建面板
            SendMessage("OpenCreate");

        }
    }
}

