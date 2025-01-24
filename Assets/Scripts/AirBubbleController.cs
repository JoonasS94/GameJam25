using UnityEngine;
using System.Collections;

public class AirBubbleController : MonoBehaviour
{
    // M‰‰ritet‰‰n sallitut rajat
    public float minY = 3.3f; // Alin korkeus
    public float maxY = 4.4f; // Ylin korkeus
    public float minX = -8.15f; // Alin X-raja
    public float maxX = 8.0f; // Ylin X-raja
    public float moveSpeed = 1.0f; // Kuplan liikkumisnopeus
    public float minCooldown = 0.1f; // Minimicooldown
    public float maxCooldown = 5f; // Maksimicooldown

    private float targetX;
    private float targetY;
    private bool isMoving = false; // Varmistaa, ett‰ liike tapahtuu vain, kun kupla ei ole jo liikkeess‰

    void Start()
    {
        // Asetetaan aloituspaikka ja k‰ynnistet‰‰n Coroutine
        targetX = Random.Range(minX, maxX);
        targetY = Random.Range(minY, maxY);
        StartCoroutine(MoveBubble());
    }

    // Coroutine liikuttamaan kuplaa
    IEnumerator MoveBubble()
    {
        while (true)
        {
            if (!isMoving)
            {
                isMoving = true;

                // Satunnainen aika, jonka kupla odottaa liikkumista ennen kuin siirtyy
                float cooldownTime = Random.Range(minCooldown, maxCooldown);
                yield return new WaitForSeconds(cooldownTime);

                // Satunnainen liike kohti uutta sijaintia
                float step = moveSpeed * Time.deltaTime;

                // Liikuta kuplaa kohti satunnaisesti valittuja koordinaatteja
                targetX = Random.Range(minX, maxX);
                targetY = Random.Range(minY, maxY);

                while (Mathf.Abs(transform.position.x - targetX) > 0.1f || Mathf.Abs(transform.position.y - targetY) > 0.1f)
                {
                    float newX = Mathf.MoveTowards(transform.position.x, targetX, step);
                    float newY = Mathf.MoveTowards(transform.position.y, targetY, step);
                    transform.position = new Vector3(newX, newY, transform.position.z);
                    yield return null; // P‰ivitt‰‰ sijainnin seuraavassa frame:ssa
                }

                // Kun liike on valmis, pysyt‰‰n paikallaan hetkisen ja siirryt‰‰n taas
                isMoving = false;
            }
        }
    }

    // T‰m‰ metodi tunnistaa osumat 3D-muotoiselle kuplalle
    private void OnTriggerEnter(Collider other)
    {
        // Tarkistetaan, osuuko pelaajan ampuma kupla (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotBubbleTag"))
        {
            Debug.Log("Pelaajan ampuma kupla osui ilmassa leijuvaan kuplaan");
        }

        // Tarkistetaan, osuuko pelaajan ampuma kupla (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotDartTag"))
        {
            Debug.Log("Pelaajan ampuma tikka osui ilmassa leijuvaan kuplaan");
        }
    }
}
