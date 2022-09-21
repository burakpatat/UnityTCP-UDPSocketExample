using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClientSend.PlayerShoot(camTransform.forward);
            ClientSend.PlayerThrowItem(camTransform.forward);
        }
    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.A),
            Input.GetKeyDown(KeyCode.Space),
        };

        ClientSend.PlayerMovement(_inputs);
    }
}
