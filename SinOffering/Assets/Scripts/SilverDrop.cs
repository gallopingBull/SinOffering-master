using UnityEngine;

/// <summary>
/// helper class that handles collision detection for silver drop
/// so player can pick silver up. 
/// </summary>

public class SilverDrop : MonoBehaviour
{
    private GameManager _gm;
    [SerializeField] int _value = 0;

    public AudioClip AudioObtainedClip;

    private void Start() => _gm = GameManager.Instance;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundManager.PlaySound(AudioObtainedClip);
            _gm.IncrementSilver(_value);
            Destroy(gameObject);
        }
    }
}