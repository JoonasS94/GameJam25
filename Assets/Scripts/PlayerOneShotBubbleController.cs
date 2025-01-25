using UnityEngine;

public class PlayerOneShotBubbleController : MonoBehaviour
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
        // Tuhotaan kupla, paitsi jos se osuu ilmakuplaan
        if (!collision.gameObject.CompareTag("AirBubbleTag") && !collision.gameObject.CompareTag("PlayerTwoTag"))
        {
            // Tuhoaa t�m�n objektin
            Destroy(gameObject);
        }
    }
}
