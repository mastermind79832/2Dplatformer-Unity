using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUiManager : MonoBehaviour
{

    private Animator[] m_anim;
    private Dictionary<GameObject,bool> m_keysValues;

    void Awake()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {
        m_keysValues = new Dictionary<GameObject, bool>();
        int KeyCount = transform.childCount;
        m_anim = new Animator[KeyCount];
        
        for (int i = 0; i < KeyCount; i++)
        {
            GameObject key = transform.GetChild(i).gameObject;
            m_keysValues.Add(key,false);   
            m_anim[i] = key.GetComponent<Animator>();
        }
    }

    public void keyCollected(int index)
    {
        GameObject key = transform.GetChild(index).gameObject;
        if(m_keysValues[key])
            return;
        m_keysValues[key] = true;
        m_anim[index].SetBool("Collected", true);
    }
}
