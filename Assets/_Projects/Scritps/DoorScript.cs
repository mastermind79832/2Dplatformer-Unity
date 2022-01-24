using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private SwitchScript switchScript;
    [SerializeField] private Animator anim;

    private bool isDoorOpen = false;
    
    public bool isDoorActive()
    {
        return isDoorOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if(switchScript.isSwtichOn() && !isDoorOpen)
        {  
            isDoorOpen = true;
            anim.SetBool("SwitchOn",true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && isDoorOpen)
        {
            Debug.Log("Game Won");
        }
    }
}
