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
        nameInput.text = GameData.user.name + "  等级:" + GameData.user.level;
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
