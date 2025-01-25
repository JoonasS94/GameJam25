using UnityEngine;

public class PlayerTwoShotBubbleController : MonoBehaviour
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
        if (collision.gameObject.CompareTag("PlatformTag") || collision.gameObject.CompareTag("WallTag") || collision.gameObject.CompareTag("PlayerOneShotBubbleTag") || collision.gameObject.CompareTag("PlayerOneShotDartTag") || collision.gameObject.CompareTag("PlayerTwoShotBubbleTag") || collision.gameObject.CompareTag("PlayerTwoShotDartTag"))
        {
            // Tuhoaa tämän objektin
            Destroy(gameObject);
        }
    }
}
