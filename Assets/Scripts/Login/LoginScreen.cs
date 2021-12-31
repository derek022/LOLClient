using GameProtocol.dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameProtocol;

public class LoginScreen : MonoBehaviour
{

    [SerializeField]
    private InputField accountInput;
    [SerializeField]
    private InputField pwdInput;

    [SerializeField]
    private Button loginBtn;


    [SerializeField]
    private GameObject registerPanel;

    private void Start()
    {
        registerPanel.SetActive(false);

        NetIO nio = NetIO.Instance;


    }

    public void OnLoginClick()
    {
        Debug.Log("�����¼");

        if(accountInput.text.Length == 0 || accountInput.text.Length >6)
        {
            Debug.Log("�˻����Ϸ�");
            return;
        }

        if ( pwdInput.text.Length < 6)
        {
            Debug.Log("���벻�Ϸ�");
            return;
        }

        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = accountInput.text;
        dto.password = pwdInput.text;

        NetIO.Instance.write(Protocol.TYPE_LOGIN, 0, LoginProtocol.LOGIN_CREQ, dto);



        loginBtn.enabled = false;


    }


    [SerializeField]
    private InputField registerAccount;
    [SerializeField]
    private InputField regPwd;
    [SerializeField]
    private InputField regConfirPwd;


    public void OnRegisterClick()
    {
        if (registerAccount.text.Length == 0 || registerAccount.text.Length > 6)
        {
            Debug.Log("�˻����Ϸ�");
            return;
        }

        if (regPwd.text.Length < 6)
        {
            Debug.Log("���벻�Ϸ�");
            return;
        }

        if(!regPwd.text.Equals(regConfirPwd.text))
        {
            Debug.Log("�����������벻�Ϸ�");
            return;
        }

        
        //WarmingManager.errors.Add(new WarningModel("ע��ʧ��",null));

    }

    public void OnShowRegisterClick()
    {
        registerPanel.SetActive(true);
    }

    public void OnCloseRegisterClick()
    {
        registerPanel.SetActive(false);
    }

}
