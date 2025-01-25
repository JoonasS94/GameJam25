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
    public float maxCooldown = 0.2f; // Maksimicooldown

    private float targetX;
    private float targetY;
    private bool isMoving = false; // Varmistaa, että liike tapahtuu vain, kun kupla ei ole jo liikkeessä
    public GameObject MoneyPrefab;
    private bool AirBubbleFirstHit = false;
    private bool AirBubbleSecondHit = false;
    private bool AirBubbleThirdHit = false;
    private bool ReadyToBurst = false;

    private GameController gameController;

    public Texture AirBubbleTexture1;
    public Texture AirBubbleTexture2;
    public Texture AirBubbleTexture3;
    public Texture AirBubbleTexture4;

    void Start()
    {
        // Asetetaan aloituspaikka ja käynnistetään Coroutine
        targetX = Random.Range(minX, maxX);
        targetY = Random.Range(minY, maxY);
        gameController = GameObject.Find("GameController")?.GetComponent<GameController>(); // Assign GameController
        StartCoroutine(MoveBubble());
    }

    // Coroutine liikuttamaan kuplaa smoothisti
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

                // Satunnainen uusi sijainti
                targetX = Random.Range(minX, maxX);
                targetY = Random.Range(minY, maxY);
                Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

                // Alkuperäinen sijainti
                Vector3 startPosition = transform.position;

                // Aika, joka liikkeeseen kuluu
                float travelTime = Vector3.Distance(startPosition, targetPosition) / moveSpeed;
                float elapsedTime = 0f;

                // Interpoloi sijaintia smoothisti
                while (elapsedTime < travelTime)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / travelTime;

                    // Käytetään Lerp:iä smoothiin liikkeeseen
                    transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0f, 1f, t));

                    yield return null; // Päivittää sijainnin seuraavassa frame:ssa
                }

                // Varmista, että sijainti asettuu tarkasti kohteeseen
                transform.position = targetPosition;

                // Liike valmis, odota seuraavaa siirtoa
                isMoving = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Tarkistetaan, osuuko pelaajan ampuma kupla (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotBubbleTag") || other.CompareTag("PlayerTwoShotBubbleTag"))
        {
            Debug.Log("Pelaajan ampuma kupla osui ilmassa leijuvaan kuplaan");
            Destroy(other.gameObject);

            // Estetään päivitykset, jos kupla on jo tuhottavissa
            if (ReadyToBurst)
            {
                Debug.Log("Kupla on jo viimeisessä tilassa. Osuma ei muuta tilaa.");
                return;
            }

            // Vaihdetaan tekstuuri osumien mukaan
            if (!AirBubbleFirstHit)
            {
                AirBubbleFirstHit = true;
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material.mainTexture = AirBubbleTexture1;
                Debug.Log("Ilmakuplaan osuttu kerran");
            }
            else if (AirBubbleFirstHit && !AirBubbleSecondHit)
            {
                AirBubbleSecondHit = true;
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material.mainTexture = AirBubbleTexture2;
                Debug.Log("Ilmakuplaan osuttu 2. kerran");
            }
            else if (AirBubbleSecondHit && !AirBubbleThirdHit)
            {
                AirBubbleThirdHit = true;
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material.mainTexture = AirBubbleTexture3;
                Debug.Log("Ilmakuplaan osuttu 3. kerran");
            }
            else if (AirBubbleThirdHit)
            {
                ReadyToBurst = true;
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material.mainTexture = AirBubbleTexture4;
                Debug.Log("Ilmakuplaan osuttu 4. kerran. Nyt voi tuhota tikalla.");
            }
        }

        // Tarkistetaan, osuuko pelaajan ampuma tikka (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotDartTag") || other.CompareTag("PlayerTwoShotDartTag"))
        {
            Debug.Log("Pelaajan ampuma tikka osui ilmassa leijuvaan kuplaan");
            Destroy(other.gameObject);

            // Mikäli ilmakupla on tuhottavissa ja siihen osuu tikka, ilmakupla tuhoutuu ja sen tilalle syntyy rahaa,
            // joka tippuu maahan.
            if (ReadyToBurst)
            {
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
