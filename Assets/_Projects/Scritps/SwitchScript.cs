using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    [Header("Logic And Connections")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject SwitchLight;

    [Header("Art")]
    [SerializeField] private Sprite switchOnSprite;
    [SerializeField] private Sprite switchOffSprite;

    private bool switchOn;
    // Start is called before the first frame update
    void Start()
    {
        switchOn = false;
        SwitchLight.SetActive(false);
        sr.sprite = switchOffSprite;
    }

    public bool isSwtichOn()
    {
        return switchOn;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {  
            switchOn = true;
            sr.sprite = switchOnSprite;
            SwitchLight.SetActive(true);
        }
    }
}
