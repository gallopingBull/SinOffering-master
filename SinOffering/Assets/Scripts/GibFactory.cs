using UnitySpriteCutter;
using UnityEngine;

/// <summary>
/// class that generates new gibs after enemies or player are critically killed. 
/// </summary>

public class GibFactory : MonoBehaviour
{
    public GameObject BloodTrailParticle;

    private void OnEnable() => GameEvents.OnEnemySliced += InitGibs;
    
    private void OnDisable() => GameEvents.OnEnemySliced -= InitGibs;
        
    private void InitGibs(SpriteCutterOutput output)
    {
        output.secondSideGameObject.transform.localScale = 
            output.firstSideGameObject.transform.localScale;

        output.firstSideGameObject.transform.tag = "Gibs";
        output.secondSideGameObject.transform.tag = "Gibs";

        output.firstSideGameObject.AddComponent<SelfDestruct>();
        output.secondSideGameObject.AddComponent<SelfDestruct>();
        output.firstSideGameObject.GetComponent<SelfDestruct>().SelfDestructTime = 5;
        output.secondSideGameObject.GetComponent<SelfDestruct>().SelfDestructTime = 5;

        GameObject tmp = GameObject.Instantiate(BloodTrailParticle, output.firstSideGameObject.transform.position, output.firstSideGameObject.transform.rotation);
        tmp.transform.parent = output.firstSideGameObject.transform;
        tmp = GameObject.Instantiate(BloodTrailParticle, output.secondSideGameObject.transform.position, output.secondSideGameObject.transform.rotation);
        tmp.transform.parent = output.secondSideGameObject.transform;

        output.firstSideGameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        output.firstSideGameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        output.firstSideGameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        output.firstSideGameObject.GetComponent<Rigidbody2D>().freezeRotation = false;

        output.firstSideGameObject.GetComponent<Rigidbody2D>().AddForce(RandomGibForce(), ForceMode2D.Force);

        #region testing whether adding force to bottom half
        //tmp.GetComponentInChildren<Rigidbody2D>().AddForce(RandomGibForce(), ForceMode2D.Force);
        //output.firstSideGameObject.layer = LayerMask.NameToLayer("Default");
        //output.secondSideGameObject.layer = LayerMask.NameToLayer("Default");
        #endregion

        Rigidbody2D newRigidbody = output.secondSideGameObject.AddComponent<Rigidbody2D>();
        newRigidbody.velocity = output.firstSideGameObject.GetComponent<Rigidbody2D>().velocity;
        newRigidbody.gravityScale = 10;
    }

    private Vector2 RandomGibForce()
    {
        return new Vector2(Random.Range(-800, 800), Random.Range(400, 800));
    }
}
