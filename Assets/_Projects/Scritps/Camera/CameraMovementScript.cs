using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    public CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Awake()
    {
        if(cam.Follow == null)
            cam.Follow = player.transform;
    }   
}
