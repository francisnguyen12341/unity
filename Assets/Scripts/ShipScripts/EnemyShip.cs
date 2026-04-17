using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyShip : Ship
{
    Vector3 targetPosition;
    private enum State{Roam,Chase,Shoot};
    [SerializeField]private float chaseDist,roamDist,shootDist;
    OutOfBounds outOfBounds;

    State currentState;
    protected override void CustomStart()
    {
        currentState = State.Roam;
        outOfBounds = GetComponent<OutOfBounds>();
        targetPosition = (Vector2)transform.position + new Vector2(Random.Range(-roamDist,roamDist),Random.Range(-roamDist,roamDist));
    }
    void Update()
    {
            if (currentState == State.Roam){
                if(outOfBounds.getDistance(transform.position,targetPosition)<1f){
                targetPosition = (Vector2)transform.position + new Vector2(Random.Range(-roamDist,roamDist),Random.Range(-
                roamDist,roamDist));
                }
                if(outOfBounds.getDistance(transform.position,PlayerShip.Instance.transform.position)<chaseDist){
                    currentState = State.Chase;
                }
            }else if(currentState == State.Chase){
                targetPosition = PlayerShip.Instance.transform.position;
                if(outOfBounds.getDistance(transform.position,PlayerShip.Instance.transform.position)<shootDist){
                currentState = State.Shoot;
            }else if(outOfBounds.getDistance(transform.position,PlayerShip.Instance.transform.position)>chaseDist*1.2f){
            currentState = State.Roam;
            }
            }else{
            targetPosition = PlayerShip.Instance.transform.position;
            if(outOfBounds.getDistance(transform.position,PlayerShip.Instance.transform.position)>shootDist){
            currentState = State.Chase;
            }
            if(canShoot){
            StartCoroutine(Shoot(moveDirection,shootForce));
            }
            }
            targetPosition = outOfBounds.getCoords(targetPosition);
            moveDirection = -outOfBounds.getDirection(transform.position,targetPosition).normalized;
    }
    protected override void Move()
    {
        if(moveDirection.magnitude > 0) {
        rigidBody.linearVelocity = moveDirection * moveSpeed;
        }else{
        rigidBody.linearVelocity -= rigidBody.linearVelocity * friction;
        }
        transform.up += ((Vector3)moveDirection-transform.up)/5;
        }
}
