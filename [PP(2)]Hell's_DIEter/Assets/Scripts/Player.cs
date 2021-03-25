using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator anim;
    private Camera cam;
    private Rigidbody rb;
    private ParticleSystem particle;

    [SerializeField] private bool isJetpackOn = false;       // 제트팩 사용 확인
    [SerializeField] private bool isImmortal = false;       // 제트팩 사용 확인
    [SerializeField] private float fuel = 100.0f;    // 비행 연료
    [SerializeField] private float weight = 100;  // 몸무게
    private int limit = 10;
    private int dumCounts = 0;
    
    [SerializeField] private float hp = 100.0f;      // 체력
    [SerializeField] private bool isGrounded;
    private bool isVisible;
    private float moveSpeed = 5.0f;
    private float rotSpeed = 10.0f;
    private float jumpForce = 10.0f;     // 점프 힘
    private float maxFuel = 1.0f;
    private int maxWeight = 100;  // 몸무게
    private int minWeight = 100;  // 몸무게

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start () {
		anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponentInParent<Rigidbody>();
        fuel = maxFuel;
        particle = gameObject.GetComponentInChildren<ParticleSystem>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update ()
    {
        // 플레이어 체력 확인 및 회복
        if (hp <= 0)
        {
            hp = 0;
            return;
        }
        if (hp > 0 && hp < 100)
        {
            hp += 1 * Time.deltaTime;
        }

        // 마우스 On/Off
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        // 체중 감량과 애니메이션
        if (Input.GetMouseButtonDown(1))
        {
            if (dumCounts != 0)
            {
                weight += -dumCounts;
                anim.SetBool("DoingExercise", true);
                if (weight < minWeight)
                    weight = minWeight;
                switch (weight)
                {
                    case 0:
                        break;
                    case 40:
                        break;
                    case 70:
                        break;
                    case 90:
                        this.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        break;
                    case 100:
                        this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        break;
                }

                cam.GetComponent<ThirdPersonCamera>().SetDistance((int)weight);
            }
        }
        else if (Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
        {
            anim.SetBool("DoingExercise", false);
        }

        MoveCharacter();    // 플레이어 이동
    }

    private void UpgradeFuelLimits()
    {
        maxFuel += 5.0f;
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
                if (fuel < maxFuel)                                     // 연료 회복
                {
                    fuel += Time.deltaTime * 0.5f;
                    if (fuel > maxFuel)                                 // 연료가 최대량보다 크면 최대량에 맞게 조정
                    {
                        fuel = maxFuel;
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

    private void OnCollisionEnter(Collision collision)
    {
        // 덤벨 획득 시 감량 하한 체중 재설정
        if(collision.gameObject.CompareTag("Dumbbell"))
        {
            dumCounts += 1; // 획득한 덤벨 개수 추가
            minWeight = maxWeight - limit * dumCounts;  // 체중 하한 재설정
            Debug.Log("Minimum weights: " + minWeight + "Kg");

            Destroy(collision.gameObject);
        }
        // 덤벨 획득 시 감량 하한 체중 재설정
        if (collision.gameObject.CompareTag("Food"))
        {
            weight = maxWeight;
            this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Destroy(collision.gameObject);
        }
    }

    public float GetFuel()
    {
        return fuel;
    }

    public int GetMaxFuel()
    {
        return (int)maxFuel;
    }

    public float GetWeight()
    {
        return weight;
    }

    public int GetMaxWeight()
    {
        return maxWeight;
    }

    public int GetHP()
    {
        return (int)hp;
    }

    public void SetHP(int damage)
    {
        if (isImmortal)
            return;

        hp -= damage;
        return;
    }

    IEnumerator SetImmortalTimer()
    {
        int timer = 0;
        isImmortal = true;
        while (timer<5)
        {
            yield return new WaitForSeconds(1.0f);
            timer++;
        }

        isImmortal = false;
        yield return null;
    }

    public bool IsStateImmortal()
    {
        return isImmortal;
    }

    public bool IsJetpackOn()
    {
        return isJetpackOn;
    }
}
