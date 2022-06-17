using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.dto;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectHandler : MonoBehaviour, IHandler
{

    private SelectRoomDTO roomDto;
    
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case SelectProtocol.DESTORY_BRO:
                SceneManager.LoadScene(1);
                break;
            
            case SelectProtocol.ENTER_SRES:
                enter(model.GetMessage<SelectRoomDTO>());
                break;
            
            case SelectProtocol.ENTER_EXBRO:
                enter(model.GetMessage<int>());
                break;
            
            case SelectProtocol.FIGHT_BRO :
                SceneManager.LoadScene(3);
                break;
            
            case SelectProtocol.READY_BRO:
                ready(model.GetMessage<SelectModel>());
                break;
                
            case SelectProtocol.SELECT_BRO:
                select(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_SRES:
                WarmingManager.errors.Add(new WarningModel("角色选择失败，请重新选择"));
                break;
                
            case SelectProtocol.TALK_BRO:
                talk(model.GetMessage<string>());
                break;
        }
    }

    private void talk(string value)
    {
        SendMessage("talkShow",value);
    }

    private void select(SelectModel model)
    {
        foreach (SelectModel item in roomDto.teamOne)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                //刷新UI界面
                SelectEventUtil.refreshView?.Invoke(roomDto);
                return;
            }
        }
        foreach (SelectModel item in roomDto.teamTwo)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                //刷新UI界面
                SelectEventUtil.refreshView?.Invoke(roomDto);
                return;
            }
        }
    }

    private void ready(SelectModel model)
    {
        if (model.userId == GameData.user.id)
        {
            SelectEventUtil.selected?.Invoke();
        }

        foreach (var item in roomDto.teamOne)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                item.ready = true;
                SelectEventUtil.refreshView?.Invoke(roomDto);
                return;
            }
        }
        
        foreach (var item in roomDto.teamTwo)
        {
            if (item.userId == model.userId)
            {
                item.hero = model.hero;
                item.ready = true;
                SelectEventUtil.refreshView?.Invoke(roomDto);
                return;
            }
        }
    }

    private void enter(SelectRoomDTO dto)
    {
        roomDto = dto;
        
        SendMessage("closeMask");

        SelectEventUtil.refreshView?.Invoke(roomDto);
    }


    private void enter(int id)
    {
        if (roomDto == null) 
            return;

        foreach (SelectModel item in roomDto.teamOne)
        {
            if (item.userId == id)
            {
                item.enter = true;

                SelectEventUtil.refreshView?.Invoke(roomDto);
                return;
            }
        }

        foreach (var item in roomDto.teamTwo)
        {
            if (item.userId == id)
            {
                item.enter = true;

                SelectEventUtil.refreshView?.Invoke(roomDto);

                return;
            }
        }
    }
}
