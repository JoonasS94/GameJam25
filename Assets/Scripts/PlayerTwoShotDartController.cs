using UnityEngine;

public class PlayerTwoShotDartController : MonoBehaviour
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
        // Tarkistetaan, onko t�rm��v� objekti merkitty 'PlatformTag'-tagilla
        if (collision.gameObject.CompareTag("PlatformTag") || collision.gameObject.CompareTag("WallTag") || collision.gameObject.CompareTag("PlayerOneShotDartTag") || collision.gameObject.CompareTag("PlayerOneShotBubbleTag") || collision.gameObject.CompareTag("PlayerTwoShotDartTag") || collision.gameObject.CompareTag("PlayerTwoShotBubbleTag"))
        {
            // Tuhoaa t�m�n objektin
            Destroy(gameObject);
        }
    }
}
