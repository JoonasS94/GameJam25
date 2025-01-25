using UnityEngine;

public class PlayerOneShotDartController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        // Tarkistetaan, onko törmäävä objekti merkitty 'PlatformTag'-tagilla
        if (!collision.gameObject.CompareTag("AirBubbleTag"))
        {
            // Tuhoaa tämän objektin
            Destroy(gameObject);
        }
    }
}
