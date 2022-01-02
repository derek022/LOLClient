using GameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 召唤师常见角色
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
            WarmingManager.errors.Add(new WarningModel("请输入正确的昵称"));
            return;
        }
        
        btn.enabled = false;
        this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.CREATE_CREQ, nameField.text);

    }
}
