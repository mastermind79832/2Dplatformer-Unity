using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private Animator[] m_anim;
    private Dictionary<GameObject,bool> m_HealthValues;
    private int healthCount;

    void Awake()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {
        m_HealthValues = new Dictionary<GameObject, bool>();
        healthCount = transform.childCount;
        m_anim = new Animator[healthCount];
        
        for (int i = 0; i < healthCount; i++)
        {
            GameObject key = transform.GetChild(i).gameObject;
            m_HealthValues.Add(key,true);   
            m_anim[i] = key.GetComponent<Animator>();
        }
    }

    public void SetHealthLost()
    {
        for (int i = 0; i < healthCount; i++)
        {
            GameObject key = transform.GetChild(i).gameObject;
            if(m_HealthValues[key])
            {   
                m_HealthValues[key] = false;
                m_anim[i].Play("Health_Lost");
                return;
            }
        }
    }

    public int GetHealthCount()
    {
        int remaining = 0;
        for (int i = 0; i < healthCount; i++)
        {
            GameObject key = transform.GetChild(i).gameObject;
            if(m_HealthValues[key])
                remaining++;
        }

        return remaining;
    }

    public void ResetHealth()
    {
        for (int i = 0; i < healthCount; i++)
        {
            GameObject key = transform.GetChild(i).gameObject;
            m_HealthValues[key] = true;
            m_anim[i].Play("Heart_Full");
        }
    }
}
