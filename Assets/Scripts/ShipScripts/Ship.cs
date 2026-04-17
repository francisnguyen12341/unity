using UnityEngine;
using System.Collections;
public abstract class Ship : MonoBehaviour
{



    public Bullet bullet;
    public string opponentTag;

    protected IEnumerator Shoot(Vector3 shootDirection,float shootForce){
        Bullet newBullet = Instantiate(bullet,transform.position,Quaternion.identity);
        newBullet.setTarget(opponentTag,shootDirection,shootForce);
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    public int health;
    [SerializeField]protected float moveSpeed, shootForce, reloadTime;
    [SerializeField]protected float friction;
    protected Vector2 moveDirection;
    protected bool canShoot = true;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        CustomStart();
    }
    abstract protected void CustomStart();
    abstract protected void Move();
    void FixedUpdate(){Move();}


    public void takeDamage(){
        health = health - 1;
        if(health<=0){Destroy(gameObject);}
    }

}