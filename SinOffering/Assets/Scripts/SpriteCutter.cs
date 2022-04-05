using System.Collections.Generic;
using UnitySpriteCutter;
using UnityEngine;

/// <summary
/// this class is used to cut sprites at runtime from player or projectile input.
/// original author: https://github.com/sabikku/unity-sprite-cutter
/// </summary>

public class SpriteCutter : MonoBehaviour
{
    public LayerMask _layermask;
    private float _sphereCastRadius = .2f;
    private Vector2 _origin, _end, _dir;

    public void Slice(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers, List<GameObject> targets = null, Vector2 dir = new Vector2())
    {
        _origin = lineStart;
        _end = lineEnd;
        _dir = dir;

        List<GameObject> gameObjectsToCut = targets;

        foreach (GameObject go in gameObjectsToCut)
        {
            if (go != null)
            {
                if (go.tag == "Gibs")
                    continue;

                if (go.tag == "Enemy")
                    go.GetComponent<EnemyController>().ChangeSprite();
            }

            SpriteCutterOutput output = UnitySpriteCutter.SpriteCutter.Cut(new SpriteCutterInput()
            {
                lineStart = lineStart,
                lineEnd = lineEnd,
                gameObject = go.transform.Find("Sprite_Enemy").gameObject,
                gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_ONE,
                //gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO
            });

            // initilaizes gibs after slice
            if (output != null && output.secondSideGameObject != null)
                GameEvents.OnEnemySliced?.Invoke(output);
        }
    }

    public void Slice(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers)
    {
        _origin = lineStart;
        _end = lineEnd;

        List<GameObject> gameObjectsToCut = new List<GameObject>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(lineStart, lineEnd, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject tmp = hit.transform.gameObject;
            print(tmp.name + " add to gameObjectsToCut<>");

            if (tmp.tag == "Enemy")
            {
                print("in enemy condiiton");
                if (gameObjectsToCut.Count == 0 && !tmp.transform.parent.GetComponent<EnemyController>().dying)
                {
                    print("added object to slice");
                    gameObjectsToCut.Add(tmp);
                    break;
                }
                if (tmp != null && !tmp.transform.parent.GetComponent<EnemyController>().dying)
                {
                    if (gameObjectsToCut.Contains(tmp))
                        continue;
                }
            }
        }

        foreach (GameObject go in gameObjectsToCut)
        {
            if (go != null)
            {
                //print(go.transform.parent.tag + " add to gameObjectsToCut<>");
                print(go.name + " add to gameObjectsToCut<>");
                if (go.tag == "Gibs")
                    continue;
                if (go.tag == "Enemy")
                    go.transform.parent.GetComponent<EnemyController>().ChangeSprite();
            }

            SpriteCutterOutput output = UnitySpriteCutter.SpriteCutter.Cut(new SpriteCutterInput()
            {
                lineStart = lineStart,
                lineEnd = lineEnd,
                gameObject = go,
                gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_ONE,
                //gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO
            });

            // initilaizes gibs after slice
            if (output != null && output.secondSideGameObject != null)
                GameEvents.OnEnemySliced?.Invoke(output);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Debug.DrawRay(_origin, _dir, Color.blue);
        Gizmos.DrawWireSphere(_end, _sphereCastRadius);
    }
}

// i might get rid of this interface if I decide to make derived classes of
// SpriteCutter instead. these derived classes will work specifically for dash attack, disc launcher, any other mechanic that cuts entities.
interface ISpriteCut
{
    public void Slice(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers, List<GameObject> targets = null, Vector2 _dir = new Vector2());
    public void Slice(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers);
}