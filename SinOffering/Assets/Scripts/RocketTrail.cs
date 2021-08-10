using UnityEngine;

public class RocketTrail : MonoBehaviour
{
    public void StopTrail() { GetComponent<ParticleSystem>().Stop(); }
}
