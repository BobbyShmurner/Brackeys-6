using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool UseAccurateMasses = true;
    public bool CanCrouch = true;

    private static GameManager m_Instance;
    public static GameManager Instance { get { return m_Instance; } }

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }
}