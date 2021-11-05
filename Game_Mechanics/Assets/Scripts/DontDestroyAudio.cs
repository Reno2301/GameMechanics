using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    private static DontDestroyAudio musicManagerInstance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (musicManagerInstance == null)
        {
            musicManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
