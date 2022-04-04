using UnityEngine;

/// <summary>
/// class for generic ammo that drops in scene after enemy is killed.
/// </summary>

public class AmmoDrop : MonoBehaviour
{
    private PlayerController _player;
    private GameManager _gm;
    public AudioClip AudioObtainedClip;

    private void Start()
    {
        _gm = GameManager.Instance;
        _player = PlayerController.instance;
        Invoke("AddToCamTargets", .1f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (_player.EquippedWeapon != null)
            {
                SoundManager.PlaySound(AudioObtainedClip); 
                _player.EquippedWeapon.GetComponent<Weapon>().ReloadWeapon();
            }
            Destroy(gameObject);
        }
    }

    private void AddToCamTargets() => _gm.camManager.AddCameraTargets(transform, .5f);
}

