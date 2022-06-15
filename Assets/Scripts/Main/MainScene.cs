using GameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private GameObject mask;  // ��ֹ�û����������������

    [SerializeField]
    private CreatePanel createPanel;

    #region ��ҽ�ɫ����
    [SerializeField]
    private Text nameInput;

    [SerializeField]
    private Slider expSlider;
    #endregion

    [SerializeField]
    private Text matchText; // ƥ�䰴ť����

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
        nameInput.text = GameData.user.name + "  �ȼ�:" + GameData.user.level;
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
    /// ƥ�䷽��
    /// </summary>
    public void match()
    {
        isMatching = !isMatching;

        matchText.text = isMatching ? "ȡ���Ŷ�" : "��ʼ�Ŷ�";
        int command = isMatching ? MatchProtocol.ENTER_CREQ : MatchProtocol.LEAVE_CREQ;
        this.WriteMessage(Protocol.TYPE_MATCH, 0, command, null);



    }
}
