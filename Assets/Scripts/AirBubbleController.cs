using UnityEngine;
using System.Collections;

public class AirBubbleController : MonoBehaviour
{
    // Määritetään sallitut rajat
    public float minY = 3.3f; // Alin korkeus
    public float maxY = 4.4f; // Ylin korkeus
    public float minX = -8.15f; // Alin X-raja
    public float maxX = 8.0f; // Ylin X-raja
    public float moveSpeed = 1.0f; // Kuplan liikkumisnopeus
    public float minCooldown = 0.1f; // Minimicooldown
    public float maxCooldown = 5f; // Maksimicooldown

    private float targetX;
    private float targetY;
    private bool isMoving = false; // Varmistaa, että liike tapahtuu vain, kun kupla ei ole jo liikkeessä
    public GameObject MoneyPrefab;
    private bool AirBubbleFirstHit = false;
    private bool AirBubbleSecondHit = false;
    private bool AirBubbleThirdHit = false;
    private bool ReadyToBurst = false;

    private GameController gameController;

    void Start()
    {
        // Asetetaan aloituspaikka ja käynnistetään Coroutine
        targetX = Random.Range(minX, maxX);
        targetY = Random.Range(minY, maxY);
        gameController = GameObject.Find("GameController")?.GetComponent<GameController>(); // Assign GameController
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
                    yield return null; // Päivittää sijainnin seuraavassa frame:ssa
                }

                // Kun liike on valmis, pysytään paikallaan hetkisen ja siirrytään taas
                isMoving = false;
            }
        }
    }

    // Tämä metodi tunnistaa osumat 3D-muotoiselle kuplalle
    private void OnTriggerEnter(Collider other)
    {
        // Tarkistetaan, osuuko pelaajan ampuma kupla (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotBubbleTag") || other.CompareTag("PlayerTwoShotBubbleTag"))
        {
            Debug.Log("Pelaajan ampuma kupla osui ilmassa leijuvaan kuplaan");
            Destroy(other.gameObject);

            // 4. osuma ilmakuplaan kuplalla
            if (AirBubbleThirdHit == true && ReadyToBurst == false)
            {
                ReadyToBurst = true;
                Debug.Log("Ilmakuplaan osuttu 4. kerran. Nyt voi tuhota tikalla.");
            }

            // 3. osuma ilmakuplaan kuplalla
            if (AirBubbleSecondHit == true)
            {
                AirBubbleThirdHit = true;
                Debug.Log("Ilmakuplaan osuttu 3. kerran.");
            }

            // 2. osuma ilmakuplaan kuplalla
            if (AirBubbleFirstHit == true)
            {
                AirBubbleSecondHit = true;
                Debug.Log("Ilmakuplaan osuttu 2. kerran");
            }

            // 1. osuma ilmakuplaan kuplalla
            if (AirBubbleFirstHit == false)
            {
                AirBubbleFirstHit = true;
                Debug.Log("Ilmakuplaan osuttu kerran");
            }
        }

        // Tarkistetaan, osuuko pelaajan ampuma tikka (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotDartTag") || other.CompareTag("PlayerTwoShotDartTag"))
        {
            Debug.Log("Pelaajan ampuma tikka osui ilmassa leijuvaan kuplaan");
            Destroy(other.gameObject);

            // Mikäli ilmakupla on tuhottavissa ja siihen osuu tikka, ilmakupla tuhoutuu ja sen tilalle syntyy rahaa,
            // joka tippuu maahan.
            if (ReadyToBurst == true)
            {
                // Check if gameController exists to prevent null reference
                if (gameController != null)
                {
                    int matchRemainingNumber = gameController.matchTimer;

                    int MoneySpawnRepeat = 1; // Default value

                    if (matchRemainingNumber > 91)
                    {
                        MoneySpawnRepeat = 1;
                    }
                    else if (matchRemainingNumber > 61 && matchRemainingNumber <= 91)
                    {
                        MoneySpawnRepeat = 2;
                    }
                    else if (matchRemainingNumber > 31 && matchRemainingNumber <= 61)
                    {
                        MoneySpawnRepeat = 3;
                    }
                    else if (matchRemainingNumber <= 31)
                    {
                        MoneySpawnRepeat = 4;
                    }

                    for (int i = 0; i < MoneySpawnRepeat; i++)
                    {
                        SpawnMoney();
                    }
                }
                else
                {
                    Debug.LogError("GameController not found!");
                }

                Destroy(gameObject);
            }
        }
    }

    // Luodaan rahaa, joka lentää ylös ja sivulle ennen kuin putoaa alas
    private void SpawnMoney()
    {
        // Luodaan rahapeliobjekti ilmakuplan sijaintiin
        GameObject money = Instantiate(MoneyPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = money.GetComponent<Rigidbody>();

        // Satunnainen voima ylöspäin ja sivuille
        Vector3 randomForce = new Vector3(
            Random.Range(-4f, 4f), // Satunnainen voima sivulle
            Random.Range(2f, 4f),  // Satunnainen voima ylöspäin
            0f); // Ei liikuta rahaa eteenpäin tai taaksepäin

        // Annetaan rahalle satunnainen voima
        rb.AddForce(randomForce, ForceMode.Impulse);
    }
}
