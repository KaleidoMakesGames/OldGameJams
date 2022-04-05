using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipSpaceModifier : MonoBehaviour
{
    public TMPro.TextMeshProUGUI field;
    [TextArea] public string spaceDownText;
    [TextArea] public string spaceUpText;
    
    // Update is called once per frame
    void Update()
    {
        field.text = Input.GetKey(KeyCode.Space) ? spaceDownText : spaceUpText;
    }
}
