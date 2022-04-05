using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CommandIndicator : MonoBehaviour
{
    public UnitCommander.CommandType mode;

    public GameObject boardModeUI;
    public GameObject moveModeUI;
    public GameObject enterModeUI;

    // Update is called once per frame
    void Update()
    {
        boardModeUI.SetActive(mode == UnitCommander.CommandType.BOARD);
        moveModeUI.SetActive(mode == UnitCommander.CommandType.MOVE);
        enterModeUI.SetActive(mode == UnitCommander.CommandType.ENTER);
    }
}
