using UnityEngine;
using System.Collections;

public class AirBubbleController : MonoBehaviour
{
    // M��ritet��n sallitut rajat
    public float minY = 3.3f; // Alin korkeus
    public float maxY = 4.4f; // Ylin korkeus
    public float minX = -8.15f; // Alin X-raja
    public float maxX = 8.0f; // Ylin X-raja
    public float moveSpeed = 1.0f; // Kuplan liikkumisnopeus
    public float minCooldown = 0.1f; // Minimicooldown
    public float maxCooldown = 5f; // Maksimicooldown

    private float targetX;
    private float targetY;
    private bool isMoving = false; // Varmistaa, ett� liike tapahtuu vain, kun kupla ei ole jo liikkeess�
    private bool destroyable = true;
    public GameObject MoneyPrefab;

    void Start()
    {
        // Asetetaan aloituspaikka ja k�ynnistet��n Coroutine
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
                    yield return null; // P�ivitt�� sijainnin seuraavassa frame:ssa
                }

                // Kun liike on valmis, pysyt��n paikallaan hetkisen ja siirryt��n taas
                isMoving = false;
            }
        }
    }

    // T�m� metodi tunnistaa osumat 3D-muotoiselle kuplalle
    private void OnTriggerEnter(Collider other)
    {
        // Tarkistetaan, osuuko pelaajan ampuma kupla (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotBubbleTag"))
        {
            Debug.Log("Pelaajan ampuma kupla osui ilmassa leijuvaan kuplaan");
            Destroy(other.gameObject);
        }

        // Tarkistetaan, osuuko pelaajan ampuma tikka (vertaa tagiin)
        if (other.CompareTag("PlayerOneShotDartTag"))
        {
            Debug.Log("Pelaajan ampuma tikka osui ilmassa leijuvaan kuplaan");
            Destroy(other.gameObject);

            // Mik�li ilmakupla on tuhottavissa ja siihen osuu tikka, ilmakupla tuhoutuu ja sen tilalle syntyy rahaa,
            // joka tippuu maahan.
            if (destroyable == true)
            {
                SpawnMoney();
                Destroy(gameObject);
            }
        }
    }

    // Luodaan rahaa, joka lent�� yl�s ja sivulle ennen kuin putoaa alas
    private void SpawnMoney()
    {
        // Luodaan rahapeliobjekti ilmakuplan sijaintiin
        GameObject money = Instantiate(MoneyPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = money.GetComponent<Rigidbody>();

        // Satunnainen voima yl�sp�in ja sivuille
        Vector3 randomForce = new Vector3(
            Random.Range(-2f, 2f), // Satunnainen voima sivulle
            Random.Range(2f, 4f),  // Satunnainen voima yl�sp�in
            0f); // Ei liikuta rahaa eteenp�in tai taaksep�in

        // Annetaan rahalle satunnainen voima
        rb.AddForce(randomForce, ForceMode.Impulse);
    }
}
