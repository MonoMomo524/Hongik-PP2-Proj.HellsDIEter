using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Animator anim;
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

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetInteger("WalkParam", 0);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        //transform.LookAt(Target);
        Vector3 vec = Target.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        q.x = 0.0f;
        q.z = 0.0f;
        transform.rotation = q;
    }
}
