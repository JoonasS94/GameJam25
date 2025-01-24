using UnityEngine;

public class PlayerOneScript : MonoBehaviour
{
    public float jumpForce = 5f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin

    void Start()
    {
        // Hae Rigidbody-komponentti
        rb = GetComponent<Rigidbody>();

        // Varmista, ett� Rigidbody l�ytyy
        if (rb == null)
        {
            Debug.LogError("Rigidbody-komponenttia ei l�ydy! Lis�� se GameObjectiin.");
        }
    }

    void Update()
    {
        // Tarkista, painetaanko A-nappia (JoystickButton0 Xbox-ohjaimessa)
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Lis�� yl�sp�in suuntautuva voima Rigidbodyyn
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Hyppy kun painetaan Xbox-one ohjaimen A-nappia");
    }

    private bool IsGrounded()
    {
        // Tarkista, koskeeko kuutio maata
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
