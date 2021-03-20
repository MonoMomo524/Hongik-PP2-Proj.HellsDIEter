using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    private Player playerScript;
    public Transform Target;
    enum AnimStates
    {
        IDLE = 0,
        ATTACK1= 1,
        ATTACK2,
        DAMAGE,
        MOVE,
        STUN,
        SEREMONY,
        DEATH

    }

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetInteger("WalkParam", 0);
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.gameObject.GetComponent<Player>();
    }

    void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        // 플레이어를 주시하도록 회전
        Vector3 vec = Target.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        q.x = 0.0f;
        q.z = 0.0f;
        transform.rotation = q;
    }

    // 플레이어가 가까이 붙으면 밀어냄
    public void PushPlayer()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
        {
            player.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*5.0f, ForceMode.Impulse);
            playerScript.SetHP(35);

            if (!playerScript.IsStateImmortal())
                player.gameObject.GetComponent<Player>().StartCoroutine("SetImmortalTimer");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Attack", true);
            PushPlayer();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Attack", false);
            player.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5.0f, ForceMode.Impulse);
        }
    }
}
