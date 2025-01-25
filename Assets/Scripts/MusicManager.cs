using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Est‰‰ objektin tuhoutumisen skenenvaihdossa
        }
        else
        {
            Destroy(gameObject); // Tuhoaa ylim‰‰r‰iset kopiot
        }
    }
}
