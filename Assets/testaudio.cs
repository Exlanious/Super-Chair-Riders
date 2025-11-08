using UnityEngine;

public class SFXTest : MonoBehaviour
{
    [SerializeField] private AudioClip kickSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Option 1: Play by index
            SFXManager.Instance.PlayMusic(0);

            // Option 2: Play by clip name
           // SFXManager.Instance.PlaySFXByName("kickSound");
        }
    }
}
