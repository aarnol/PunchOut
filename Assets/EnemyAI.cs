using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
// add gore?

public class EnemyAI : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    private enum State
    {
        Idle,
        PunchingRight,
        PunchingLeft,
        Blocking,
        Dead,
        Staggered
    }
    private State currentState = State.Idle;
    
    private float lastActionTime = 0.0f;
    private float actionCooldown = 3f;
    
    private float blockDuration = 2.0f;
    private int health = 8;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        AppEventManager.PlayerOnPunch += Punched;
        AppEventManager.Death += Death;
    }
    private void OnDisable()
    {
        AppEventManager.PlayerOnPunch -= Punched;
        AppEventManager.Death -= Death;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            currentState = State.Dead;
            
            
            AppEventManager.Death.Invoke(0);

        }
       

        if (Time.time - lastActionTime > actionCooldown && currentState == State.Idle)
        {
            int randomState = Random.Range(0, 3); // 0: PunchingRight, 1: PunchingLeft, 2: Blocking
            switch (randomState)
            {
            case 0:
                currentState = State.PunchingRight;
                lastActionTime = Time.time;
                StartCoroutine(Punch(true));
                Debug.Log("Enemy is punching right");
                StartCoroutine(Punch(true));
                break;
            case 1:
                currentState = State.PunchingLeft;
                lastActionTime = Time.time;
                StartCoroutine(Punch(false));
                Debug.Log("Enemy is punching left");
                
                break;
            case 2:
                currentState = State.Blocking;
                lastActionTime = Time.time;
                StartCoroutine(Block());
                break;
            }
        }
    }

    private void Death(int id)
    {   
        if(id == 0){
            Debug.Log("Enemy died");
            Destroy(gameObject);
            Destroy(leftHand);
            Destroy(rightHand);
        }
    
            
        
    }
    private IEnumerator Punch(bool isRight)
    {
        
        if (isRight)
        {
           
           rightHand.transform.DOMove(rightHand.transform.position + Vector3.forward * 100f, 0.5f).SetEase(Ease.OutQuint).SetLoops(2, LoopType.Yoyo);
            
            
            
        }
        else
        {
            
            leftHand.transform.DOMove(leftHand.transform.position + Vector3.forward * 100f, 0.5f).SetEase(Ease.OutQuint).SetLoops(2, LoopType.Yoyo);

            
        }
        yield return new WaitForSeconds(0.75f);
        if(isRight){
            AppEventManager.EnemyOnPunch.Invoke(this, true);
        }
        else{
            AppEventManager.EnemyOnPunch.Invoke(this, false);
        }
        yield return new WaitForSeconds(0.75f);
        
        
        
        currentState = State.Idle;
    }

    private IEnumerator Block()
    {
        Debug.Log("Enemy is blocking");
        rightHand.transform.rotation = Quaternion.Euler(0, 90, 0);
        leftHand.transform.rotation = Quaternion.Euler(0, 90, 0);
        leftHand.transform.position = leftHand.transform.position + new Vector3(0, 0, 150f);
        yield return new WaitForSeconds(blockDuration);
        rightHand.transform.rotation = Quaternion.Euler(0, 180, 0);
        leftHand.transform.rotation = Quaternion.Euler(0, 0, 0);
        leftHand.transform.position = leftHand.transform.position - new Vector3(0, 0, 150f);
        currentState = State.Idle;
    }

    private void Punched(Component component, bool isLeft)
    {
        if (currentState == State.Blocking)
        {
            Debug.Log("Enemy blocked the punch");
        }
        if(currentState == State.Idle)
        {
            Debug.Log("Enemy got punched");
            StartCoroutine(RotateBackwards());
            health--;
        }
    }
    

    

    private IEnumerator RotateBackwards()
    {
        currentState = State.Staggered;
        float duration = 2.0f; // duration of the rotation
      

        
        transform.DOShakePosition(duration, 100f, 10, 90, false, true);
        yield return new WaitForSeconds(duration);

  
        currentState = State.Idle;
    }
    

}
