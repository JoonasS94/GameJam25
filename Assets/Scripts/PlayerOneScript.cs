using UnityEngine;
using TMPro;

public class PlayerOneScript : MonoBehaviour
{
    public float jumpForce = 6f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin
    public float horizontalInput;
    private float playerSpeed = 6.0f;
    public int playerOneMoney;
    public TextMeshProUGUI playerOneMoneyText;

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
        // Lis�� yl�sp�in suuntautuva voima Rigidbodyyn
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

    void OnCollisionEnter(Collision collision)
    {
        // Tarkistetaan, onko t�rm��v� objekti merkitty 'PlatformTag'-tagilla
        if (collision.gameObject.CompareTag("MoneyTag"))
        {
            // Tuhoaa raha objektin
            Destroy(collision.gameObject);

            Debug.Log("rahaa pelaajalle");
            playerOneMoney += 1;
            playerOneMoneyText.text = "P1 Money: " + playerOneMoney;
        }
    }

}
