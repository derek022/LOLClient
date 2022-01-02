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
        Debug.Log("申请登录");

        if(accountInput.text.Length == 0 || accountInput.text.Length >6)
        {
            Debug.Log("账户不合法");
            return;
        }

        if ( pwdInput.text.Length < 6)
        {
            return;
        }

        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = accountInput.text;
        dto.password = pwdInput.text;

        this.WriteMessage(Protocol.TYPE_LOGIN, 0, LoginProtocol.LOGIN_CREQ, dto);



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
            Debug.Log("账户不合法");
            return;
        }

        if (regPwd.text.Length < 6)
        {
            Debug.Log("密码不合法");
            return;
        }

        if(!regPwd.text.Equals(regConfirPwd.text))
        {
            Debug.Log("两次输入密码不合法");
            return;
        }

        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = registerAccount.text;
        dto.password = regPwd.text;

        this.WriteMessage(Protocol.TYPE_LOGIN, 0, LoginProtocol.REG_CREQ, dto);

        //WarmingManager.errors.Add(new WarningModel("注册失败",null));
        OnCloseRegisterClick();
    }


    public void openLoginBtn()
    {
        accountInput.text = string.Empty;
        pwdInput.text = string.Empty;

        loginBtn.enabled = true;
    }

    public void OnShowRegisterClick()
    {
        registerPanel.SetActive(true);
    }

    public void OnCloseRegisterClick()
    {
        registerAccount.text = string.Empty;
        regPwd.text = string.Empty;
        regConfirPwd.text = string.Empty;

        registerPanel.SetActive(false);
    }

}
