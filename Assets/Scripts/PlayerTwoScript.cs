using UnityEngine;
using TMPro;

public class PlayerTwoScript : MonoBehaviour
{
    public float jumpForce = 6f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin
    private float horizontalInput;
    private float playerSpeed = 6.0f;
    public int playerTwoMoney;
    public TextMeshProUGUI playerTwoMoneyText;

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
        horizontalInput = Input.GetAxis("P2_Horizontal");

        // Tarkista, painetaanko X-nappia (JoystickButton0 PlayStation-ohjaimessa)
        if (Input.GetButtonDown("P2_Jump") && IsGrounded())
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
        Debug.Log("Hyppy kun painetaan 2. ohjaimen X-nappia");
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

    void OnCollisionEnter(Collision collision)
    {
        // Tarkistetaan, onko törmäävä objekti merkitty 'PlatformTag'-tagilla
        if (collision.gameObject.CompareTag("MoneyTag"))
        {
            // Tuhoaa raha objektin
            Destroy(collision.gameObject);

            Debug.Log("rahaa pelaajalle");
            playerTwoMoney += 1;
            playerTwoMoneyText.text = "P1 Money: " + playerTwoMoney;
        }
    }

}
