using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    private PlayerController player;
    private GameManager gm;
    public AudioClip audioObtainedClip;

    private void Start()
    {
        gm = GameManager.instance;
        player = PlayerController.instance;
        Invoke("AddToCamTargets", .1f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (player.EquippedWeapon != null)
            {
                SoundManager.PlaySound(audioObtainedClip); 
                player.EquippedWeapon.GetComponent<Weapon>().ReloadWeapon();
            }
            Destroy(gameObject);
        }
    }

    private void AddToCamTargets()
    {
        gm.camManager.AddCameraTargets(transform, .5f);
        /*
        if (MultiTargetCam.instance.enabled &&
            MultiTargetCam.instance.AddCrates)
        {
            MultiTargetCam.instance.targets.Add(gameObject.transform);
        }*/
    }
}
