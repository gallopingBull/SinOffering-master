using UnityEngine;

public class LightFlash : MonoBehaviour
{
    private Light light;
    public float ActiveTime = .1f;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        Invoke("TurnOffLight", ActiveTime);
    }

    // Update is called once per frame
    void TurnOffLight()
    {
        light.intensity = 0;
    }
}
