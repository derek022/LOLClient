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

    [SerializeField]
    private Text nameInput;

    [SerializeField]
    private Slider expSlider;

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
}
