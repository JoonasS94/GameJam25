using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    // M‰‰ritet‰‰n sallitut rajat ilmakuplan synnyinalueeksi
    public float minY = 3.3f; // Alin korkeus
    public float maxY = 4.4f; // Ylin korkeus
    public float minX = -8.15f; // Alin X-raja
    public float maxX = 8.0f; // Ylin X-raja

    public GameObject AirBubblePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour on luotu
    void Start()
    {
        SpawnAirBubble(); // Luodaan ensimm‰inen ilmakupla heti pelin alussa
        StartCoroutine(AirBubbleSpawn()); // K‰ynnistet‰‰n korutiini kuplien luomista varten
    }

    // Update on kutsuttu kerran per kehyksess‰
    void Update()
    {
        // T‰ss‰ ei en‰‰ tarvita mit‰‰n erityist‰ logiikkaa, koska korutiini huolehtii kuplan luomisesta
    }

    IEnumerator AirBubbleSpawn()
    {
        while (true) // T‰m‰ pit‰‰ korutiinin k‰ynniss‰ jatkuvasti
        {
            // Lasketaan kuinka monta objektille, jolla on "AirBubbleTag", on olemassa
            GameObject[] airBubbles = GameObject.FindGameObjectsWithTag("AirBubbleTag");

            // Jos ei ole ilmakuplia, luo uusi
            if (airBubbles.Length == 0)
            {
                yield return new WaitForSeconds(1); // Odotetaan satunnainen m‰‰r‰ aikaa
                SpawnAirBubble(); // Luodaan uusi ilmakupla
            }
            else
            {
                // Jos on ilmakupla, odota v‰h‰n ennen seuraavaa tarkistusta
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void SpawnAirBubble()
    {
        // Liikuta kuplaa kohti satunnaisesti valittuja koordinaatteja
        float targetX = Random.Range(minX, maxX); // Satunnainen X-koordinaatti
        float targetY = Random.Range(minY, maxY); // Satunnainen Y-koordinaatti

        // K‰ytet‰‰n Vector3:ta suoraan
        Vector3 AirBubbleSpawnPosition = new Vector3(targetX, targetY, 0);

        // Instantiate the air bubble at the calculated position
        GameObject airBubble = Instantiate(AirBubblePrefab, AirBubbleSpawnPosition, Quaternion.identity);

        // Aseta instansoidulle kuplalle tagi, jotta se voidaan tunnistaa
        airBubble.tag = "AirBubbleTag";
    }
}
