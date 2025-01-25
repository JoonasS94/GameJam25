using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerOneScript : MonoBehaviour
{
    public float PlayerOnejumpForce = 6f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin
    private float horizontalInput;
    public float PlayerOneplayerSpeed = 6.0f;
    public int playerOneMoney;
    public TextMeshProUGUI playerOneMoneyText;

    public GameController GameControllerScript;

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
        horizontalInput = Input.GetAxis("P1_Horizontal");

        // Tarkista, painetaanko A-nappia (JoystickButton0 Xbox-ohjaimessa)
        if (Input.GetButtonDown("P1_Jump") && IsGrounded())
        {
            Jump();
        }

        // Pelaajan liikkuminen vasemmalle tai oikealle toteutus
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * PlayerOneplayerSpeed);
    }

    private void Jump()
    {
        // Lisää ylöspäin suuntautuva voima Rigidbodyyn
        rb.AddForce(Vector3.up * PlayerOnejumpForce, ForceMode.Impulse);
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
        // Tarkistetaan, onko törmäävä objekti merkitty 'PlatformTag'-tagilla
        if (collision.gameObject.CompareTag("MoneyTag"))
        {
            // Tuhoaa raha objektin
            Destroy(collision.gameObject);

            Debug.Log("rahaa pelaajalle");
            playerOneMoney += 1;
            playerOneMoneyText.text = "P1 Money: " + playerOneMoney;
        }

        if (collision.gameObject.CompareTag("PlayerOneShotDartTag"))
        {
            StartCoroutine(StunPlayerOne());
            // Tuhoaa pelaajaan osuneen tikan, mutta hidastetaan vihua hyvin lyhyen aikaa
            Destroy(collision.gameObject);
        }
    }

    IEnumerator StunPlayerOne()
    {
        Debug.Log("Tikka osui ja p2 hidastettu");
        PlayerOnejumpForce = 1f;
        PlayerOneplayerSpeed = 1f;
        yield return new WaitForSeconds(0.3f);
        if (GameControllerScript.matchEnded == false)
        {
            PlayerOnejumpForce = 6f;
            PlayerOneplayerSpeed = 6f;
        }
        else
        {
            PlayerOnejumpForce = 0;
            PlayerOneplayerSpeed = 0;
        }
    }
}
