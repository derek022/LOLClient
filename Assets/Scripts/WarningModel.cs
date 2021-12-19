using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void WarningResult();

public class WarningModel
{
    public WarningResult result;
    public string value;

    public WarningModel(string value,WarningResult result)
    {
        this.result = result;
        this.value = value;
    }
}
