using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerTwoScript : MonoBehaviour
{
    public float PlayerTwojumpForce = 7f; // Hyppyvoima, muokattavissa editorissa
    private Rigidbody rb; // Viittaus Rigidbody-komponenttiin
    private float horizontalInput;
    public float PlayerTwoplayerSpeed = 6.0f;
    public int playerTwoMoney;
    public TextMeshProUGUI playerTwoMoneyText;

    public GameController GameControllerScript;

    public AudioSource PlayerTwoScriptAudioSource;
    public AudioClip[] PlayerTwoScriptAudioClipArray;

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
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * PlayerTwoplayerSpeed);
    }

    private void Jump()
    {
        // Lisää ylöspäin suuntautuva voima Rigidbodyyn
        rb.AddForce(Vector3.up * PlayerTwojumpForce, ForceMode.Impulse);
        Debug.Log("Hyppy kun painetaan 2. ohjaimen X-nappia");
        PlayerTwoScriptAudioSource.PlayOneShot(RandomJumpSoundClip());
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
            playerTwoMoneyText.text = "P2 Money: " + playerTwoMoney;
        }

        if (collision.gameObject.CompareTag("PlayerOneShotDartTag"))
        {
            StartCoroutine(StunPlayerTwo());
            // Tuhoaa pelaajaan osuneen tikan, mutta hidastetaan vihua hyvin lyhyen aikaa
            Destroy(collision.gameObject);
        }
    }

    IEnumerator StunPlayerTwo()
    {
        Debug.Log("Tikka osui ja p2 hidastettu");
        PlayerTwoScriptAudioSource.PlayOneShot(RandomHurtSoundClip());
        PlayerTwojumpForce = 1f;
        PlayerTwoplayerSpeed = 1f;
        yield return new WaitForSeconds(0.3f);
        if (GameControllerScript.matchEnded == false)
        {
            PlayerTwojumpForce = 7f;
            PlayerTwoplayerSpeed = 6f;
        }
        else
        {
            PlayerTwojumpForce = 0;
            PlayerTwoplayerSpeed = 0;
        }
    }

    AudioClip RandomJumpSoundClip()
    {
        return PlayerTwoScriptAudioClipArray[Random.Range(0, 1)];
    }

    AudioClip RandomHurtSoundClip()
    {
        return PlayerTwoScriptAudioClipArray[Random.Range(2, 6)];
    }
}
