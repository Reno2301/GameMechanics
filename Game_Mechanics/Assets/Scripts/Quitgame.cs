using UnityEngine;

public class Quitgame : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
            Debug.Log("Quit!");
        }
    }
}
