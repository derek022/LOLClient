using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmingManager : MonoBehaviour
{

    public static List<WarningModel> errors = new List<WarningModel>();

    [SerializeField]
    private WarmingWindow window;


    private void Update()
    {
        if(errors.Count > 0)
        {
            WarningModel err = errors[0];
            errors.Remove(err);
            window.Active(err);
        }
    }
}
