using UnityEngine;

public class PlayerOneScript : MonoBehaviour
{
    public float jumpForce = 8f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin
    public float horizontalInput;
    private float playerSpeed = 10.0f;

    void Start()
    {
        // Hae Rigidbody-komponentti
        rb = GetComponent<Rigidbody>();

        // Varmista, että Rigidbody löytyy
        if (rb == null)
        {
            Debug.LogError("Rigidbody-komponenttia ei löydy! Lisää se GameObjectiin.");
        }
    }

    void Update()
    {
        // Pelaajan tatin liikkuminen vasemmalle tai oikealle
        horizontalInput = Input.GetAxis("Horizontal");
        float xRaw = Input.GetAxisRaw("Horizontal");

        // Tarkista, painetaanko A-nappia (JoystickButton0 Xbox-ohjaimessa)
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && IsGrounded())
        {
            Jump();
        }

        // Pelaajan liikkuminen vasemmalle tai oikealle toteutus
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerSpeed);

    }

    private void Jump()
    {
        // Lisää ylöspäin suuntautuva voima Rigidbodyyn
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Hyppy kun painetaan Xbox-one ohjaimen A-nappia");
    }

    private bool IsGrounded()
    {
        // Suorita RaycastAll ja hae kaikki osumat
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, 1.1f);

        foreach (var hit in hits)
        {
            // Tarkista, osuuko maa-objektiin (tarkista Tag "PlatformTag")
            if (hit.collider.CompareTag("PlatformTag"))
            {
                return true;
            }
        }

        // Jos ei osunut maahan, palautetaan false
        return false;
    }

}
