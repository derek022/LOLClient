using GameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ٻ�ʦ������ɫ
/// </summary>
public class CreatePanel : MonoBehaviour
{
    [SerializeField]
    private InputField nameField;

    [SerializeField]
    private Button btn;

    public void open()
    {
        btn.enabled = true;
        gameObject.SetActive(true);
    }

    public void close()
    {
        btn.enabled = false;
        gameObject.SetActive(false);
    }

    public void click()
    {
        if(nameField.text.Length > 6 || nameField.text == string.Empty)
        {
            WarmingManager.errors.Add(new WarningModel("��������ȷ���ǳ�"));
            return;
        }
        
        btn.enabled = false;
        this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.CREATE_CREQ, nameField.text);

    }
}
