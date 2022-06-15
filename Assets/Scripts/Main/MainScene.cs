using GameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private GameObject mask;  // 防止用户误操作，顶层遮罩

    [SerializeField]
    private CreatePanel createPanel;

    #region 玩家角色数据
    [SerializeField]
    private Text nameInput;

    [SerializeField]
    private Slider expSlider;
    #endregion

    [SerializeField]
    private Text matchText; // 匹配按钮文字

    private bool isMatching = false;

    private void Start()
    {
        if(GameData.user == null)
        {
            mask.SetActive(true);

            this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.INFO_CREQ, null);
        }
    }

    public void RefershView()
    {
        nameInput.text = GameData.user.name + "  等级:" + GameData.user.level;
        expSlider.value = GameData.user.exp / 100;

        isMatching = false;
    }


    void OpenCreate()
    {
        createPanel.open();
    }

    void CloseCreate()
    {
        createPanel.close();
    }


    void closeMask()
    {
        mask.SetActive(false);
    }


    /// <summary>
    /// 匹配方法
    /// </summary>
    public void match()
    {
        isMatching = !isMatching;

        matchText.text = isMatching ? "取消排队" : "开始排队";
        int command = isMatching ? MatchProtocol.ENTER_CREQ : MatchProtocol.LEAVE_CREQ;
        this.WriteMessage(Protocol.TYPE_MATCH, 0, command, null);



    }
}
