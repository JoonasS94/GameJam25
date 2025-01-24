using UnityEngine;

public class PlayerOneGunController : MonoBehaviour
{
    public Transform weapon; // Ase, jota liikutetaan

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
    }
}