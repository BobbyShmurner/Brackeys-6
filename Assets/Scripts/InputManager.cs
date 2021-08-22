using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

class InputManager : MonoBehaviour
{
    Inputs input;

    public static Vector2 mousePos;
    public static Vector2 mouseDelta;

    private static InputManager m_Instance;
    public static InputManager Instance { get { return m_Instance; } }

    private void Awake()
    {
        // Singleton Code

        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }

        // Normal Code

        input = new Inputs();

        input.Mouse.Position.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
        input.Mouse.Position.canceled += ctx => mousePos = Vector2.zero;

        input.Mouse.Delta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        input.Mouse.Delta.canceled += ctx => mouseDelta = Vector2.zero;

        input.Enable();
    }
}
