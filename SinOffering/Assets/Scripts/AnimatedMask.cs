//https://gitlab.com/snippets/1785639

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMask : MonoBehaviour
{
    // The mask you want to animate
    public SpriteMask mask;

    // The GameObject that has the animation
    public SpriteRenderer targetRenderer;

    // It's important to use LateUpdate.
    // If you use Update, the sprite mask will be 1 frame behind.
    void LateUpdate()
    {
        if (targetRenderer != null)
        {
            if (mask.sprite != targetRenderer.sprite)
            {
                mask.sprite = targetRenderer.sprite;
                if (targetRenderer.flipX)
                {
                    mask.transform.eulerAngles = new Vector3(mask.gameObject.transform.eulerAngles.x, 180, mask.gameObject.transform.eulerAngles.z);
                }
                else
                {
                    mask.transform.eulerAngles = new Vector3(mask.gameObject.transform.eulerAngles.x, 0, mask.gameObject.transform.eulerAngles.z);

                }
            }
        }
    }
}
