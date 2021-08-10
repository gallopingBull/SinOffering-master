
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Entity {
    #region variables
    
    public bool isAggro;
    public bool isGhost = false;
    [HideInInspector]
    public bool dying;
    public bool InArena = false;
    //[HideInInspector]
    public bool CanMove = true;
    public bool SpawnAmmoCrates = true;

    public float FireDamageRate = .75f;
    public float FireDamageAmmount = 1f;
    public int MaxFireTime = 100;

    public float AggroSpeedMultiplier = 1.5f;
    public int AggroLifeMultiplier = 2;

    private float min = 2;
    private float max = 3;
    private bool onFire = false;



    public AudioClip EnemyKilledClip;
    public ParticleSystem PS_Fire;
    public ParticleSystem PS_Smoke;
    public GameObject AmmoDrop;
    public GameObject PS_BloodExplosion;
    public GameObject GibsPrefab;
    public Light _light;
    public Sprite slicedSprite;
    
    private GameObject Target;
    #endregion

    #region functions
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (!isGhost && !gm.paused)
        {
            CheckIfFalling();
            GravityModifier();
            CheckFloor();
        }
    }
    private void Update()
    {
        if (gm.paused)
            return;
        
        if (CanMove)
        {
            if (isGhost) { Fly(); }
            else { Movement(); }
        }

        //spawn dust trails
        if (IsGrounded && !isGhost)
        {
            if (EnableDustTrails)
            {
                if (!canSpawnDustTrail)
                {
                    if (stepRate > 0)
                    {
                        //print("counting down until next dust taril spawns");
                        stepRate -= Time.deltaTime;
                        return;
                    }
                    else
                    {
                        //print("spawn dust trail");
                        canSpawnDustTrail = true;
                        SpawnDustTrail();
                    }
                }
            }
        }
    }

    public void ChangeSprite()
    {
        //print("change sprite called");
        GetComponentInChildren<SpriteRenderer>().sprite = slicedSprite;
    }

    protected override void InitActor()
    {
        Target = GameObject.Find("Player");
        if (isAggro)
        {
            Speed = Speed * AggroSpeedMultiplier;
            life = life * AggroLifeMultiplier;
            EnableFire(); 
            //GetComponentInChildren<SpriteRenderer>().color = Color.red; 
        }
    }

    public void Suicide()
    {
        if (!dying)
        {
            dying = true;
            SoundManager.PlaySound(EnemyKilledClip);
            camManager.RemoveCameraTargets(gameObject.transform);
            BloodActorSprite.gameObject.SetActive(false);
            DeParentCaller();


            print("entity (" + gameObject.name + ") getting destoryed - Suicide()");
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        if (!dying)
        {
            dying = true;
            SoundManager.PlaySound(EnemyKilledClip);
            if (Target != null)
            {
                Instantiate(GibsPrefab, transform.position, transform.rotation);

                BloodActorSprite.gameObject.SetActive(false);
                DeParentCaller();
                Target.GetComponent<PlayerController>().CurEnemyKills++;
                camManager.RemoveCameraTargets(gameObject.transform);

                print("entity (" + gameObject.name + ") getting destoryed- Explode()");
                Destroy(gameObject);
            }
        }
    }

    public override void Killed()
    {
        if (!dying) {
            dying = true;
            SoundManager.PlaySound(EnemyKilledClip);
            if (Target != null)
            { 
                Instantiate(PS_BloodExplosion, transform.position, transform.rotation);

                Target.GetComponent<PlayerController>().CurEnemyKills++;

                BloodActorSprite.gameObject.SetActive(false);
                DeParentCaller();
                camManager.RemoveCameraTargets(gameObject.transform);

                print("entity ("+gameObject.name+ ") getting destoryed- Killed()");
                Destroy(gameObject);
            }
        }
    }

    public void DashKilled()
    {
        if (!dying)
        {
            dying = true;
            if (Target != null)
            {
                //GetComponentInChildren<SpriteRenderer>().sprite = slicedSprite;
                SoundManager.PlaySound(EnemyKilledClip);
               
                //spawn ammo drop
                if (SpawnAmmoCrates)
                    Instantiate(AmmoDrop, transform.position, transform.rotation);
                
                GameObject tmp;
                tmp = Instantiate(PS_BloodExplosion, transform.position, transform.rotation);
                tmp.GetComponent<BloodSplat>().AssignParent(gameObject.transform);

                camManager.RemoveCameraTargets(gameObject.transform);

                BloodActorSprite.gameObject.SetActive(false);
                DeParentCaller();

                Target.GetComponent<PlayerController>().CurEnemyKills++;

                print("entity (" + gameObject.name + ") getting destoryed- DashKilled()");
                Destroy(gameObject);
            }
        }
    }

    private void Fly()
    {
        if (Target != null)
        {
            float oldXPos = transform.position.x;
            if (Vector2.Distance(transform.position, Target.transform.position) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                    Target.transform.position, 
                    Speed * (Time.deltaTime * TimeScale.enemies));
            }

            if (ActorSprite != null)
            {
                if (transform.position.x > oldXPos)
                    ActorSprite.GetComponent<SpriteRenderer>().flipX = true;
                if (transform.position.x < oldXPos)
                    ActorSprite.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void Movement()
    {
        Vector3 tmp; 

        if (!facingLeft)
        {
            tmp = Vector3.right;
            dir = 1;
        }

        else
        {
            tmp = -Vector3.right;
            dir = -1;
        }
           
        transform.Translate(tmp * Speed * (Time.fixedDeltaTime*TimeScale.enemies));
        
        if (ActorSprite != null)
        {
            ActorSprite.GetComponent<SpriteRenderer>().flipX = facingLeft;
            BloodActorSprite.GetComponent<SpriteRenderer>().flipX = facingLeft;

        }
    }

    //sets an enemy on fire from projectile
    public void EnableFire()
    {
        if (!onFire)
        {
            onFire = true;
            if (!PS_Fire.isPlaying)
            {
                PS_Fire.Play();
                PS_Smoke.Play();
                _light.gameObject.SetActive(true);
            }

            if (!isAggro)
                StartCoroutine(FireDamage());
        }
        return;
    }

    private IEnumerator FireDamage()
    {
        int curtime = 0;
        while (onFire)
        {
            curtime++;
            yield return new WaitForSeconds(FireDamageRate);
            Damage(FireDamageAmmount);

            if (curtime >= MaxFireTime)
                break;
        }
        onFire = false; 
        PS_Fire.Stop();
        PS_Smoke.Stop();
        StopCoroutine(FireDamage());
    }
#endregion
}
