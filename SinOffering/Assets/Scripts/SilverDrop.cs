using UnityEngine;

public class SilverDrop : MonoBehaviour
{
    private GameManager gm;
    [SerializeField]
    private int value = 0;

    public AudioClip audioObtainedClip;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundManager.PlaySound(audioObtainedClip);
            gm.IncrementSilver(value);
            Destroy(gameObject);
        }
    }
}