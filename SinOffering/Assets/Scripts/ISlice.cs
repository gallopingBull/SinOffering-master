//https://github.com/sabikku/unity-sprite-cutter


using System.Collections;
using System.Collections.Generic;
using UnitySpriteCutter;
using UnityEngine;

public class ISlice : MonoBehaviour
{
    public LayerMask _layermask;
    public GameObject BloodTrailParticle;
    public List<GameObject> gameObjectsToCut;
    Vector2 origin, end, dir;
    private float sphereCastRadius = .2f;

    public void Slice(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers, List<GameObject> targets = null, Vector2 _dir = new Vector2())
    {
      
        origin = lineStart;
        end = lineEnd;
        dir = _dir;
        //print("Slice() - lineStart: " + lineStart);
        //print("Slice() - lineEnd: " + lineEnd);

        List<GameObject> gameObjectsToCut = targets;

        foreach (GameObject go in gameObjectsToCut)
        {
            if (go != null)
            {
                //print(go.transform.parent.tag + " add to gameObjectsToCut<>");
                //print(go.name + " add to gameObjectsToCut<>");
                if (go.tag == "Gibs")
                {
                    //print("is gibs");
                    continue;
                }
                if (go.tag == "Enemy")
                {
                    //print("changing enemy sprite
                    //go.GetComponentInParent<EnemyController>().ChangeSprite();
                    go.GetComponent<EnemyController>().ChangeSprite();
                }
            }

            SpriteCutterOutput output = SpriteCutter.Cut(new SpriteCutterInput()
            {
                lineStart = lineStart,
                lineEnd = lineEnd,
                gameObject = go.transform.Find("Sprite_Enemy").gameObject, 
                gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_ONE,
                //gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO
            });


            if (output != null && output.secondSideGameObject != null)
            {
                //initilaizes gibs after slice
                InitGibs(output, go);
            }
        }
    }



    public void DiscSlice(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers)
    {

        origin = lineStart;
        end = lineEnd;


        List<GameObject> gameObjectsToCut = new List<GameObject>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(lineStart, lineEnd, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject tmp = hit.transform.gameObject;
            print(tmp.name + " add to gameObjectsToCut<>");

            if (tmp.tag == "Enemy")
            {
                print("in enemy condiiton");
                if (gameObjectsToCut.Count == 0 &&
                    !tmp.transform.parent.GetComponent<EnemyController>().dying)
                {
                    print("added object to slice");
                    gameObjectsToCut.Add(tmp);
                    break;
                }
                if (tmp != null &&
                    !tmp.transform.parent.GetComponent<EnemyController>().dying)
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
                {
                    //print("is gibs");
                    continue;
                }
                if (go.tag == "Enemy")
                {
                    //print("changing enemy sprite
                    //go.GetComponentInParent<EnemyController>().ChangeSprite();
                    go.transform.parent.GetComponent<EnemyController>().ChangeSprite();
                }
            }

            SpriteCutterOutput output = SpriteCutter.Cut(new SpriteCutterInput()
            {
                lineStart = lineStart,
                lineEnd = lineEnd,
                gameObject = go,
                gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_ONE,
                //gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO
            });


            if (output != null && output.secondSideGameObject != null)
            {
                //initilaizes gibs after slice
                InitGibs(output, go);
            }
        }
    }

    bool HitCounts(RaycastHit2D hit)
    {
        return (hit.transform.GetComponent<SpriteRenderer>() != null ||
                 hit.transform.GetComponent<MeshRenderer>() != null);
    }

    private void InitGibs(SpriteCutterOutput output, GameObject go)
    {
        output.secondSideGameObject.transform.localScale = output.firstSideGameObject.transform.localScale;
        output.firstSideGameObject.transform.tag = "Gibs";
        output.secondSideGameObject.transform.tag = "Gibs";

        output.firstSideGameObject.AddComponent<SelfDestruct>();
        output.secondSideGameObject.AddComponent<SelfDestruct>();
        output.firstSideGameObject.GetComponent<SelfDestruct>().SelfDestructTime = 5;
        output.secondSideGameObject.GetComponent<SelfDestruct>().SelfDestructTime = 5;


        GameObject tmp = GameObject.Instantiate(BloodTrailParticle, output.firstSideGameObject.transform.position, transform.rotation);
        tmp.transform.parent = output.firstSideGameObject.transform;
        tmp = GameObject.Instantiate(BloodTrailParticle, output.secondSideGameObject.transform.position, transform.rotation);
        tmp.transform.parent = output.secondSideGameObject.transform; 

        output.firstSideGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        output.firstSideGameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        output.firstSideGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        output.firstSideGameObject.GetComponent<Rigidbody2D>().freezeRotation = false;


        output.firstSideGameObject.GetComponent<Rigidbody2D>().AddForce(RandomGibForce(), ForceMode2D.Force);

        //tmp = go;
        //print(tmp.name);
        //tmp.GetComponentInChildren<Rigidbody2D>().AddForce(RandomGibForce(), ForceMode2D.Force);
        //go.GetComponentInChildren<Rigidbody2D>().AddForce(RandomGibForce(), ForceMode2D.Force);

        //output.firstSideGameObject.layer = LayerMask.NameToLayer("Default");
        //output.secondSideGameObject.layer = LayerMask.NameToLayer("Default");

        Rigidbody2D newRigidbody = output.secondSideGameObject.AddComponent<Rigidbody2D>();
        newRigidbody.velocity = output.firstSideGameObject.GetComponent<Rigidbody2D>().velocity;
        newRigidbody.gravityScale = 10;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
       
        Debug.DrawRay(origin, dir, Color.blue);
       Gizmos.DrawWireSphere(end, sphereCastRadius);
    }
    private Vector2 RandomGibForce()
    {
        return new Vector2(Random.Range(-800, 800), Random.Range(400, 800));
    }
    
    //test function
    public void RemoveEnemyFromLists(GameObject enemy)
    {   
        print("removing " + enemy.name);
        //change the reference to player here
        if (PlayerController.instance.GetComponent<DashCommand>().targets.Contains(enemy))
            PlayerController.instance.GetComponent<DashCommand>().targets.Remove(enemy);
    }
}
