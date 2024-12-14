using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

// add vibration on hit
public class blocking : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftGlove;
    public GameObject rightGlove;
    
    private bool isBlocking;
    public float punchThreshold = 1.0f; 
    public float punchCooldown = 1.5f;
    private float lastPunchTime = 0.0f;

    public int health = 8;
    // Update is called once per frame
    private Vector3 lastLeftGlovePosition;
    private Vector3 lastRightGlovePosition;
    public InputActionReference reset;
    private enum State
    {
        Idle,
        Dead,
        Staggered
    }
    private State currentState = State.Idle;
    void Start()
    {
        lastLeftGlovePosition = leftGlove.transform.position;
        lastRightGlovePosition = rightGlove.transform.position;

    }
    void OnEnable()
    {
        AppEventManager.EnemyOnPunch += Punched;
        reset.action.performed += ResetScene; // Subscribe once in Start()
        
    }

    void OnDisable()
    {
        AppEventManager.EnemyOnPunch -= Punched;
    }
    void Update()
    {
        if(health <= 0)
        {
            currentState = State.Dead;
            Debug.Log("Player is dead");
            AppEventManager.Death.Invoke(1);
            Destroy(rightGlove);
            Destroy(leftGlove);
        }
        if(currentState == State.Idle)
        {
            if (Vector3.Dot(rightHand.transform.forward, Vector3.up) > 0.9f && Vector3.Dot(leftHand.transform.forward, Vector3.up) > 0.9f)
            {
                isBlocking = true;
            }
            else
            {
                isBlocking = false;
            }

            Vector3 leftGloveVelocity = (leftGlove.transform.position - lastLeftGlovePosition) / Time.deltaTime;
            Vector3 rightGloveVelocity = (rightGlove.transform.position - lastRightGlovePosition) / Time.deltaTime;

            lastLeftGlovePosition = leftGlove.transform.position;
            lastRightGlovePosition = rightGlove.transform.position;

            if (Time.time - lastPunchTime > punchCooldown)
            {
                if (leftGloveVelocity.magnitude > punchThreshold && Vector3.Dot(leftGloveVelocity.normalized, leftHand.transform.forward) > 0.9f)
                {
                    
                    AppEventManager.PlayerOnPunch.Invoke(this, true);
                    // leftGlove.transform.DOMove(rightHand.transform.position + Vector3.forward * 200, 0.5f).SetEase(Ease.OutQuint).SetLoops(2, LoopType.Yoyo);
                    lastPunchTime = Time.time;
                }
                else if (rightGloveVelocity.magnitude > punchThreshold && Vector3.Dot(rightGloveVelocity.normalized, rightHand.transform.forward) > 0.9f)
                {
                    AppEventManager.PlayerOnPunch.Invoke(this, false);
                    // rightGlove.transform.DOMove(rightHand.transform.position + Vector3.forward * 200, 0.5f).SetEase(Ease.OutQuint).SetLoops(2, LoopType.Yoyo);
                    lastPunchTime = Time.time;
                }
            }
        }

        }
    void Punched(Component enemy, bool isRight){
        if(!isBlocking){
            this.transform.DOShakePosition(.75f, 50, 20, 90, false);
            health--;
            Debug.Log("Player: " + health);
            currentState = State.Staggered;
            
            StartCoroutine(Stagger());
            
        }
    }
    private IEnumerator Stagger()
    {
        yield return new WaitForSeconds(0.75f);
        currentState = State.Idle;
    }
    void ResetScene(InputAction.CallbackContext context)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        
    }

        
}
