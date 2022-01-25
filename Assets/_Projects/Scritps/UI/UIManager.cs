using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public KeyUiManager keyUI;
    public void KeysObtained(int index)
    {
        keyUI.keyCollected(index);
    }
    
}
