using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerOneScript : MonoBehaviour
{
    public float PlayerOnejumpForce = 7f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin
    private float horizontalInput;
    public float PlayerOneplayerSpeed = 6.0f;
    public int playerOneMoney;
    public TextMeshProUGUI playerOneMoneyText;

    public GameController GameControllerScript;

    public AudioSource PlayerOneScriptAudioSource;
    public AudioClip[] PlayerOneScriptAudioClipArray;

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
        // Lis�� yl�sp�in suuntautuva voima Rigidbodyyn
        rb.AddForce(Vector3.up * PlayerOnejumpForce, ForceMode.Impulse);
        Debug.Log("Hyppy kun painetaan Xbox-one ohjaimen A-nappia");
        PlayerOneScriptAudioSource.PlayOneShot(RandomJumpSoundClip());
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

        if (collision.gameObject.CompareTag("PlayerTwoShotDartTag"))
        {
            StartCoroutine(StunPlayerOne());
            // Tuhoaa pelaajaan osuneen tikan, mutta hidastetaan vihua hyvin lyhyen aikaa
            Destroy(collision.gameObject);
        }
    }

    IEnumerator StunPlayerOne()
    {
        Debug.Log("Tikka osui ja p2 hidastettu");
        PlayerOneScriptAudioSource.PlayOneShot(RandomHurtSoundClip());
        PlayerOnejumpForce = 1f;
        PlayerOneplayerSpeed = 1f;
        yield return new WaitForSeconds(0.3f);
        if (GameControllerScript.matchEnded == false)
        {
            PlayerOnejumpForce = 7f;
            PlayerOneplayerSpeed = 6f;
        }
        else
        {
            PlayerOnejumpForce = 0;
            PlayerOneplayerSpeed = 0;
        }
    }

    AudioClip RandomJumpSoundClip()
    {
        return PlayerOneScriptAudioClipArray[Random.Range(0, 1)];
    }

    AudioClip RandomHurtSoundClip()
    {
        Debug.Log("kuuluko");
        return PlayerOneScriptAudioClipArray[Random.Range(2, 6)];
    }
}
