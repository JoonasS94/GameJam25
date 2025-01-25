using UnityEngine;
using System.Collections;
using TMPro;

public class GameController : MonoBehaviour
{
    // Määritetään sallitut rajat ilmakuplan synnyinalueeksi
    public float minY = 3.3f; // Alin korkeus
    public float maxY = 4.4f; // Ylin korkeus
    public float minX = -8.15f; // Alin X-raja
    public float maxX = 8.0f; // Ylin X-raja
    public int matchTimer = 120;

    public GameObject AirBubblePrefab;
    public GameObject P1MoneyInfo;
    public GameObject P2MoneyInfo;
    public TextMeshProUGUI TimerText;
    public bool matchEnded = false;

    public PlayerOneScript playerOneScript;
    public PlayerTwoScript playerTwoScript;

    public GameObject instructionText;
    public TextMeshProUGUI instructionTextTMP;

    // Start is called once before the first execution of Update after the MonoBehaviour on luotu
    void Start()
    {
        StartCoroutine(StartInfo());
    }

    // Update on kutsuttu kerran per kehyksessä
    void Update()
    {
        if (matchEnded == false && matchTimer == 0)
        {
            matchEnded = true;
            P1MoneyInfo.SetActive(false);
            P2MoneyInfo.SetActive(false);
            TimerText.text = "Time!";
            playerOneScript.PlayerOneplayerSpeed = 0f;
            playerOneScript.PlayerOnejumpForce = 0f;
            playerTwoScript.PlayerTwoplayerSpeed = 0f;
            playerTwoScript.PlayerTwojumpForce = 0f;
            StartCoroutine(WinnerAnnoucement());
        }
    }

    IEnumerator AirBubbleSpawn()
    {
        while (matchTimer > 0) // Tämä pitää korutiinin käynnissä ottelun päättymiseen saakka
        {
            // Lasketaan kuinka monta objektille, jolla on "AirBubbleTag", on olemassa
            GameObject[] airBubbles = GameObject.FindGameObjectsWithTag("AirBubbleTag");

            // Jos ei ole ilmakuplia, luo uusi
            if (airBubbles.Length == 0)
            {
                yield return new WaitForSeconds(1); // Odotetaan satunnainen määrä aikaa
                SpawnAirBubble(); // Luodaan uusi ilmakupla
            }
            else
            {
                // Jos on ilmakupla, odota vähän ennen seuraavaa tarkistusta
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void SpawnAirBubble()
    {
        // Liikuta kuplaa kohti satunnaisesti valittuja koordinaatteja
        float targetX = Random.Range(minX, maxX); // Satunnainen X-koordinaatti
        float targetY = Random.Range(minY, maxY); // Satunnainen Y-koordinaatti

        // Käytetään Vector3:ta suoraan
        Vector3 AirBubbleSpawnPosition = new Vector3(targetX, targetY, 0);

        // Instantiate the air bubble at the calculated position
        GameObject airBubble = Instantiate(AirBubblePrefab, AirBubbleSpawnPosition, Quaternion.identity);

        // Aseta instansoidulle kuplalle tagi, jotta se voidaan tunnistaa
        airBubble.tag = "AirBubbleTag";
    }

    IEnumerator MatchTimeLimit()
    {
        while (matchTimer > 0)
        {
            yield return new WaitForSeconds(1);
            matchTimer -= 1;
            TimerText.text = "Time remaining: " + matchTimer;
        }
    }

    IEnumerator WinnerAnnoucement()
    {
        yield return new WaitForSeconds(1.5f);
        TimerText.text = "And the winner is...";

        yield return new WaitForSeconds(3f);
        if (playerOneScript.playerOneMoney > playerTwoScript.playerTwoMoney)
        {
            TimerText.text = "PLAYER 1!!!";
        }
        if (playerOneScript.playerOneMoney < playerTwoScript.playerTwoMoney)
        {
            TimerText.text = "PLAYER 2!!!";
        }
        if (playerOneScript.playerOneMoney == playerTwoScript.playerTwoMoney)
        {
            TimerText.text = "TIE!!!";
        }
    }

    IEnumerator StartInfo()
    {
        yield return new WaitForSeconds(5f);
        instructionTextTMP.text = "READY?";
        yield return new WaitForSeconds(1);
        instructionTextTMP.text = "3...";
        yield return new WaitForSeconds(1);
        instructionTextTMP.text = "2..";
        yield return new WaitForSeconds(1);
        instructionTextTMP.text = "1.";
        yield return new WaitForSeconds(1);
        instructionTextTMP.text = "GO!";
        yield return new WaitForSeconds(1);
        instructionText.SetActive(false);
        SpawnAirBubble(); // Luodaan ensimmäinen ilmakupla heti pelin alussa
        StartCoroutine(AirBubbleSpawn()); // Käynnistetään korutiini kuplien luomista varten
        StartCoroutine(MatchTimeLimit());
    }
}
