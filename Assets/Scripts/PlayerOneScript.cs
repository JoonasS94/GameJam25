using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOneScript : MonoBehaviour
{
    public InputActionReference jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("A-nappia painettu!");
        }
    }
}
