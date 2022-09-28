using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputs : MonoBehaviour
{
    private ChareterControler inputs;


    private void Awake()
    {
        inputs = new ChareterControler();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    public Vector3 Move()
    {
        Vector2 moveInputvalue = inputs.Player.Move.ReadValue<Vector2>();

        Vector3 vector3 = new Vector3(moveInputvalue.x, 0f, moveInputvalue.y);

        return vector3;
    }

    public bool Jump()
    {
        return inputs.Player.Jump1.triggered;
    }

    public bool Sprint()
    {
        return inputs.Player.Sprint.IsPressed();
    }
}
