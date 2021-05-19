using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour, IMonsterFSM
{
    public Transform[] points;
    private WeightPuzzleController controller;
    public enum STATE
    {
        ROAMING,
        FLEEING,
        CATCHED,
        ISOLATED
    }
    public STATE Estate;

    private GameObject target;
    private Animator anim;
    private Vector3 direction;
    public NavMeshAgent agent;

    private float walkSpeed = 3.0f;
    private float runSpeed = 7.0f;
    private float distance;
    int destPoint;
    private bool isGrabbing;
    public bool IsGrabbing
    {
        get { return isGrabbing; }
        set { isGrabbing = value; }
    }
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindObjectOfType<WeightPuzzleController>().GetComponent<WeightPuzzleController>();
        if (controller == null)
            Debug.LogError("ERROR");
        anim = anim = gameObject.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        target = GameObject.Find("Player");
        audio = GetComponent<AudioSource>();

        if(points.Length == 0)
        {
            Debug.Log("RETURN");
            return;
        }
        destPoint = Random.Range(0, points.Length);

        // 무작위로 시작 포지션 지정
        transform.position = points[destPoint].position;
        GotoNextPoint();
    }

    void Update()
    {
        if (points == null || points.Length < 2)
            return;

        if (Estate == STATE.CATCHED
            || Estate == STATE.ISOLATED)
        {
            audio.Stop();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        //&& Estate == STATE.ROAMING)
        {
            GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        // 랜덤으로 갈 곳을 선택
        int currentPoint = destPoint;
        while (destPoint == currentPoint)
        {
            destPoint = Random.Range(0, points.Length);
        }

        // 선택된 포인트로 가도록 함
        agent.destination = points[destPoint].position;
    }

    public void IRoaming()
    {
        if (audio.isPlaying == true)
            audio.Stop();
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsRunning", false);
        anim.speed = 1.0f;
        agent.speed = walkSpeed;
    }

    public void IFlee()
    {
        if (audio.isPlaying == false)
            audio.Play();
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsWalking", false);
        anim.speed = 1.5f;
        agent.speed = runSpeed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //target = other.gameObject;
            //float distance = Vector3.Distance(transform.position, target.transform.position);

            //if (distance < 25.0f)
            //{
            //    IFlee();
            //}
            if (agent != null) 
                IFlee();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IRoaming();
    }

    public void MoveTo(Vector3 position)
    {
        this.transform.position = position;
    }
}