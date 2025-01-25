using UnityEngine;
using System.Collections;

public class PlayerTwoGunController : MonoBehaviour
{
    public Transform weapon; // Ase, jota liikutetaan
    public bool PlayerTwoFiringCooldown = false;
    public GameObject PlayerTwoShotBubblePrefab;
    public GameObject PlayerTwoShotDartPrefab;
    public Transform PlayerTwoWeaponMuzzle;
    private float ShotPower = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        for (int i = 0; i < 20; i++) // Testaa napit 0-19
        {
            if (Input.GetKeyDown($"joystick 2 button {i}"))
            {
                Debug.Log($"Joystick 2 Button {i} painettu.");
            }
        }


        // Lue oikean tatin syöte
        float horizontal = Input.GetAxis("Joystick2.Joystick Axis 4"); // Tatista vaakasuuntainen liike
        float vertical = Input.GetAxis("Joystick2.Joystick Axis 5"); // Tatista pystysuuntainen liike

        // Tarkista, onko tatti liikkeessä
        if (horizontal != 0 || vertical != 0)
        {
            // Laske haluttu kulma
            float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg * -1 - 90;

            // Aseta aseen rotaatio välittömästi
            weapon.rotation = Quaternion.Euler(0, 0, angle);
        }




        // Lue R2-arvo
        float rightTrigger = Input.GetAxis("P2_RightTrigger");

        // Tarkista, painetaanko R2:sta
        if (rightTrigger > 0.1f)
        {
            Debug.Log("R2 painettu, arvo: " + rightTrigger);
            FirePlayerTwoWeaponBubble();
        }

        // Lue L2-arvo
        float leftTrigger = Input.GetAxis("P2_LeftTrigger");

        // Tarkista, painetaanko RT:tä
        if (leftTrigger > 0.1f)
        {
            Debug.Log("L2 painettu, arvo: " + leftTrigger);
            FirePlayerTwoWeaponDart();
        }
    }

    void FirePlayerTwoWeaponBubble()
    {
        if (PlayerTwoFiringCooldown == false)
        {
            PlayerTwoFiringCooldown = true;
            StartCoroutine(PlayerTwoFiringCooldownTimer());

            // Instansioi kupla aseen piipusta ja huomioi aseen rotaatio
            GameObject createdPlayerTwoBubble = Instantiate(PlayerTwoShotBubblePrefab, PlayerTwoWeaponMuzzle.position, PlayerTwoWeaponMuzzle.rotation);

            // Kuplan Rb
            Rigidbody PlayerTwoBubbleRigidbody = createdPlayerTwoBubble.GetComponent<Rigidbody>();

            // Kuplaan energia jolla lähtee liikkeelle aseen piipusta eteenpain
            PlayerTwoBubbleRigidbody.AddForce(PlayerTwoWeaponMuzzle.up * ShotPower, ForceMode.Impulse);
        }
    }

    void FirePlayerTwoWeaponDart()
    {
        if (PlayerTwoFiringCooldown == false)
        {
            PlayerTwoFiringCooldown = true;
            StartCoroutine(PlayerTwoFiringCooldownTimer());

            // Instansioi tikka aseen piipusta ja huomioi aseen rotaatio
            GameObject createdPlayerTwoDart = Instantiate(PlayerTwoShotDartPrefab, PlayerTwoWeaponMuzzle.position, PlayerTwoWeaponMuzzle.rotation);

            // Tikka Rb
            Rigidbody PlayerTwoDartRigidbody = createdPlayerTwoDart.GetComponent<Rigidbody>();

            // Kuplaan energia jolla lähtee liikkeelle aseen piipusta eteenpain
            PlayerTwoDartRigidbody.AddForce(PlayerTwoWeaponMuzzle.up * ShotPower, ForceMode.Impulse);
        }
    }

    IEnumerator PlayerTwoFiringCooldownTimer()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerTwoFiringCooldown = false;
    }
}
