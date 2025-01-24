using UnityEngine;
using System.Collections;

public class PlayerOneGunController : MonoBehaviour
{
    public Transform weapon; // Ase, jota liikutetaan
    public bool PlayerOneFiringCooldown = false;
    public GameObject PlayerOneShotBubblePrefab;
    public Transform PlayerOneWeaponMuzzle;
    private float ShotPower = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Lue oikean tatin syöte
        float horizontal = Input.GetAxis("Joystick Axis 4"); // Tatista vaakasuuntainen liike
        float vertical = Input.GetAxis("Joystick Axis 5"); // Tatista pystysuuntainen liike

        // Tarkista, onko tatti liikkeessä
        if (horizontal != 0 || vertical != 0)
        {
            // Laske haluttu kulma
            float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg * -1 - 90;

            // Aseta aseen rotaatio välittömästi
            weapon.rotation = Quaternion.Euler(0, 0, angle);
        }




        // Lue RT (R2)-arvo
        float rightTrigger = Input.GetAxis("RightTrigger");

        // Tarkista, painetaanko RT:tä
        if (rightTrigger > 0.1f)
        {
            Debug.Log("RT painettu, arvo: " + rightTrigger);
            FirePlayerOneWeaponBubble();
        }
    }

    void FirePlayerOneWeaponBubble()
    {
        if (PlayerOneFiringCooldown == false)
        {
            PlayerOneFiringCooldown = true;
            StartCoroutine(PlayerOneFiringCooldownTimer());

            // Instansioi kupla aseen piipusta ja huomioi aseen rotaatio
            GameObject createdPlayerOneBubble = Instantiate(PlayerOneShotBubblePrefab, PlayerOneWeaponMuzzle.position, PlayerOneWeaponMuzzle.rotation);

            // Kuplan Rb
            Rigidbody PlayerOneBubbleRigidbody = createdPlayerOneBubble.GetComponent<Rigidbody>();

            // Kuplaan energia jolla lähtee liikkeelle aseen piipusta eteenpain
            PlayerOneBubbleRigidbody.AddForce(PlayerOneWeaponMuzzle.up * ShotPower, ForceMode.Impulse);
        }
    }

    IEnumerator PlayerOneFiringCooldownTimer()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerOneFiringCooldown = false;
    }
}