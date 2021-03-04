using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator anim;
    private Camera cam;
    private Rigidbody rb;
    private ParticleSystem particle;

    [SerializeField] private bool isJetpackOn;       // 제트팩 사용 확인
    [SerializeField] private float fuel = 10.0f;      // 비행 연료
    [SerializeField] private bool isGrounded;
    private float gravity = 14.0f;
    private float speed = 5.0f;
    private float jumpForce = 10.0f;     // 점프 힘
    private float maximumFuel;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start () {
		anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponentInParent<Rigidbody>();
        maximumFuel = fuel;
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
        if (particle == null)
        {
            Debug.Log("CANNOT FIND JETPACK PARTICLES.");
        }
    }

    void Update ()
    {
        MoveCharacter();
    }

    private void UpgradeFuelLimits()
    {
        maximumFuel += 5.0f;
        return;
    }

    private void MoveCharacter()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;

        // 점프 처리
        if (isGrounded)  // 점프중이 아님
        {
            // 에니매이션 처리
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) // Walk foward, left, right.
                anim.SetInteger("WalkParam", 1);
            else if (Input.GetKey(KeyCode.S)) // Walk backward
                anim.SetInteger("WalkParam", 2);
            else // Idle
                anim.SetInteger("WalkParam", 0);

            anim.SetInteger("JumpParam", 0);
            anim.SetBool("UsingJetpack", false);

            if (Input.GetKeyDown(KeyCode.Space)) // 점프
            {
                isGrounded = false;
                anim.SetInteger("JumpParam", 1);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        else // 점프 또는 낙하 중
        {
            anim.SetInteger("WalkParam", 0);
            // 연료가 남아있으면 제트팩 사용
            if (Input.GetKey(KeyCode.Space) && fuel > 0.0f)
            {
                isJetpackOn = true;
            }
            else
            {
                isJetpackOn = false;
            }
        }
        // 제트팩 사용 처리
        UsingJetpack();

        // 이동 처리
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            this.transform.forward = lookForward;
            this.transform.position += moveDir * Time.deltaTime * 5f;
        }
    }

    private void UsingJetpack()
    {
        switch (isJetpackOn)
        {
            case true:
                anim.SetBool("UsingJetpack", true);
                particle.Play();
                fuel -= Time.deltaTime;
                rb.AddForce(Vector3.up * jumpForce * 2, ForceMode.Force);
                break;
            case false:
                anim.SetBool("UsingJetpack", false);
                particle.Stop();
                // 연료 회복
                if (fuel < maximumFuel)
                {
                    fuel += Time.deltaTime * 0.1f;
                    if (fuel > maximumFuel)
                        fuel = maximumFuel;
                }
                
                break;
        }
        
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetInteger("JumpParam", 0);    // 점프 에니메이션 OFF
            anim.SetBool("UsingJetpack", false);    // 제트팩 에니메이션 OFF
            isJetpackOn = false;                // 제트팩 사용 중 아님
            isGrounded = true;
        }

        return;
    }
}
