using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour, IMonsterFSM
{
    public static int level = 1;
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
    public Transform destination;
    private Vector3 direction;

    private float walkSpeed = 2.0f;
    private float runSpeed = 4.0f;
    private float distance;
    int destPoint;
    private bool isGrabbing;
    public bool IsGrabbing
    {
        get { return isGrabbing; }
        set { isGrabbing = value; }
    }
    private bool run;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindObjectOfType<WeightPuzzleController>().GetComponent<WeightPuzzleController>();
        anim = anim = gameObject.GetComponentInChildren<Animator>();
        target = GameObject.Find("Player");
        audio = GetComponent<AudioSource>();

        if(points.Length == 0)
            return;
        destPoint = Random.Range(0, points.Length);

        // 무작위로 시작 포지션 지정
        run = false;
        destination = points[destPoint];
        GotoNextPoint();
    }

    void Update()
    {
        if (points == null || points.Length < 2)
            return;

        if (Estate == STATE.CATCHED || Estate == STATE.ISOLATED)
        {
            audio.Stop();
            return;
        }

        // 목적지에 도착하면 다음 목적지 세팅
        if (this.transform.position.x == destination.position.x
            && this.transform.position.z == destination.position.z)
        {
            GotoNextPoint();
        }

        // 자연스러운 이동을 위한 회전
        direction = destination.position - this.transform.position;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(direction), 1.5f * Time.deltaTime);

        if (run)
            IFlee();
        else
            IRoaming();
    }

    // 목적지 선택 메소드
    void GotoNextPoint()
    {
        // 랜덤으로 목적지를 선택
        int currentPoint = destPoint;
        while (destPoint == currentPoint)
        {
            destPoint = Random.Range(0, points.Length);
        }

        destination = points[destPoint];
    }

    // 목적지까지 남은 거리 측정 메소드
    public float RemainingDistance()
    {
        float dist = Vector3.Distance(this.transform.position, points[destPoint].transform.position);
        return dist;
    }

    public void IRoaming()
    {
        if (audio.isPlaying == false)
            audio.Play();
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsRunning", false);
        anim.speed = 0.8f;
        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * walkSpeed);
    }

    public void IFlee()
    {
        if (audio.isPlaying == false)
            audio.Play();
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsWalking", false);
        anim.speed = 1.25f;
        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * runSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Estate != STATE.CATCHED)
        {
            Estate = STATE.FLEEING;
            IFlee();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Estate = STATE.ROAMING;
            IRoaming();
        }
    }

    public void MoveTo(Vector3 position)
    {
        this.transform.position = position;
    }
}