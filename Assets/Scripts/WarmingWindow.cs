using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarmingWindow : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private WarningResult result;

    public void Active(WarningModel value)
    {
        text.text = value.value;
        this.result = value.result;
        gameObject.SetActive(true);
    }


    public void OnConfirmClick()
    {
        gameObject.SetActive(false);
        this.result?.Invoke();
    }

    public void close()
    {
        gameObject.SetActive(false);
        if(result!=null)
        {

        }


    }
}
