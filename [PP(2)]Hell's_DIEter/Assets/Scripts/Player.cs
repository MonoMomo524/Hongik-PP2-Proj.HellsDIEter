using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator anim;
    private Camera cam;
    private Rigidbody rb;
    private ParticleSystem particle;

    [SerializeField] private bool isJetpackOn;       // 제트팩 사용 확인
    [SerializeField] private float fuel = 100.0f;    // 비행 연료
    [SerializeField] private float weight = 100.0f;  // 몸무게
    [SerializeField] private float hp = 100.0f;      // 체력
    [SerializeField] private bool isGrounded;
    private float gravity = 14.0f;
    private float moveSpeed = 5.0f;
    private float rotSpeed = 10.0f;
    private float jumpForce = 10.0f;     // 점프 힘
    private float maximumFuel = 10;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start () {
		anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponentInParent<Rigidbody>();
        fuel = maximumFuel;
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
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
        bool isMove = moveInput.magnitude != 0; // 이동 입력 판별(True면 이동, False면 정지)

        // 점프 판별
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
            // 점프 애니메이션 후 연료가 남아있으면 제트팩 사용
            if (Input.GetKey(KeyCode.Space) && fuel > 0.0f &&
                (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump Up") ||
                anim.GetCurrentAnimatorStateInfo(0).IsName("Float")) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isJetpackOn = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space) || fuel <= 0.0f)
            {
                isJetpackOn = false;
            }
        }
        
        UsingJetpack();     // 제트팩 사용 처리
        
        if (isMove)         // 이동 처리
        {
            Vector3 lookForward = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized; // 전방향 확인
            Vector3 lookRight = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;       // 우방향 확인
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;                              // 캐릭터가 움직일 방향 계산

            Quaternion rotateFoward = Quaternion.LookRotation(lookForward);     // 플레이어가 앞을 바라보고
            rb.rotation = Quaternion.Slerp(rb.rotation, rotateFoward, rotSpeed * Time.deltaTime);    // 부드럽게 회전
            this.transform.position += moveDir * Time.deltaTime * moveSpeed;    // 움직일 방향을 향해 나아감
        }
    }

    private void UsingJetpack()
    {
        switch (isJetpackOn)
        {
            case true:
                anim.SetBool("UsingJetpack", true);                         // 제트팩 사용 애니메이션 실행
                particle.Play();                                            // 불꽃 파티클 실행
                fuel -= Time.deltaTime;                                     // 연료 사용
                rb.AddForce(Vector3.up * jumpForce * 2, ForceMode.Force);   // 제트팩 작동(상승) 부분
                break;
                
            case false:
                anim.SetBool("UsingJetpack", false);                        // 제트팩 사용 애니메이션 중단
                particle.Stop();                                            // 불꽃 파티클 중단
                if (fuel < maximumFuel)                                     // 연료 회복
                {
                    fuel += Time.deltaTime * 0.5f;
                    if (fuel > maximumFuel)                                 // 연료가 최대량보다 크면 최대량에 맞게 조정
                    {
                        fuel = maximumFuel;
                    }
                }
                break;
        }
        
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            anim.SetInteger("JumpParam", 0);    // 점프 에니메이션 OFF
            anim.SetBool("UsingJetpack", false);    // 제트팩 에니메이션 OFF
            isJetpackOn = false;                // 제트팩 사용 중 아님
            isGrounded = true;
        }

        return;
    }

    public float GetFuelInfo()
    {
        return fuel;
    }

    public float GetMaxFuelInfo()
    {
        return maximumFuel;
    }

    public float GetWeightInfo()
    {
        return weight;
    }

    public float GetHPInfo()
    {
        return hp;
    }
}
