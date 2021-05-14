using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour {

    #region Variables
    private Animator anim;
    private Camera cam;
    private Rigidbody rb;
    private ParticleSystem particle;
    private Color originColor;
    public SkinnedMeshRenderer body;
    private GameObject grab;

    private bool isJetpackOn = false;       // 제트팩 사용 확인
    public bool IsJetpackOn
    {
        get { return isJetpackOn; }
    }

    private bool isImmortal = false;       // 제트팩 사용 확인
    public bool IsImmortal
    {
        get { return isImmortal; }
    }

    private float fuel = 1.0f;    // 비행 연료
    public float Fuel
    {
        get { return fuel; }
    }

    private float maxFuel = 1.0f;   // 연료량
    public float MaxFuel
    {
        get { return (int)maxFuel; }
        set { maxFuel += value; }
    }

    private float weight = 100;  // 몸무게
    public float Weight
    {
        get { return weight; }
        set { weight += value; }
    }

    private int maxWeight = 100;  // 증량 가능 몸무게
    public int MaxWeight
    {
        get { return maxWeight; }
    }

    private int minWeight = 100;  // 감량 가능 몸무게
    public int MinWeight
    {
        get { return minWeight; }
        set { minWeight -= value; }
    }

    private float hp = 100.0f;                // 체력
    public float Hp
    {
        get { return (int)hp; }
        set
        {
            if (isImmortal) { return; }
            else {
                hp -= value;
                StartCoroutine(SetImmortalTimer());
            }
        }
    }

    private bool usable;                          // 마우스를 자유롭게 사용 가능한지
    public bool Usable
    {
        get { return usable; }
        set { usable = value; }
    }

    private int dumCounts = 0;              // 덤벨 개수
    public int DumCounts
    {
        get { return dumCounts; }
    }

    private int coinCounts = 0;              // 동전 개수
    public int CoinCounts
    {
        get { return coinCounts; }
    }

    private bool isGrabbing = false;        // 몬스터를 잡고있는지
    public bool IsGrabbing
    {
        get { return isGrabbing; }
    }

    [SerializeField]private bool isGrounded;                 // 지면에 있는지
    private float moveSpeed = 5.0f;     // 캐릭터 이동 속도
    private float rotSpeed = 10.0f;      // 캐릭터 회전 속도
    private float jumpForce = 10.0f;    // 점프 힘

    private bool isWalking;
    private AudioSource audio;
    #endregion

    void Start ()
    {
        cam = Camera.main;
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponentInParent<Rigidbody>();
        fuel = maxFuel;
        particle = gameObject.GetComponentInChildren<ParticleSystem>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        usable = true;
        isWalking = false;
        audio = GetComponent<AudioSource>();

        originColor = body.materials[1].color;
        
    }

    void Update ()
    {
        if(isWalking)
        {
            if(audio.isPlaying==false)
            {
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }

        // 마우스 On/Off
        if(Input.GetKeyDown(KeyCode.LeftControl) && usable)
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
            LoosingWeight();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("DoingExercise", false);
        }

        MoveCharacter();    // 플레이어 이동

        // 잡기
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * 10.0f, Color.blue, 0.3f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f)
                && hit.transform.CompareTag("Slime"))
            {
                isGrabbing = !isGrabbing;

                if (isGrabbing)
                {
                    grab = hit.transform.gameObject;
                    if (grab.transform.Find("Body").CompareTag("Slime"))
                        grab.GetComponent<Slime>().Estate = Slime.STATE.CATCHED;

                    grab.transform.SetParent(this.gameObject.transform);
                }
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f)
                && hit.transform.CompareTag("Scale")
                && grab != null)
            {
                hit.transform.GetComponent<Scale>().CheckMonster();
                isGrabbing = false;
                grab = null;
            }
        }

        if (grab != null)
        {
            GrabObject();
        }
    }

    // 체중 감소메소드
    private void LoosingWeight()
    {
        if (dumCounts != 0)
        {
            weight += -dumCounts;
            anim.SetBool("DoingExercise", true);
            if (weight < minWeight)
                weight = minWeight;

            // 체중에 따라 플레이어 사이즈와 능력치 변경
            ResizingWeight(weight);

            //cam.GetComponent<ThirdPersonCamera>().SetDistance((int)weight);
        }
    }

    private void ResizingWeight(float weight)
    {
        switch (weight)
        {
            case 0:
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                moveSpeed = 10.0f;
                jumpForce = 17.5f;
                break;
            case 40:
                this.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                moveSpeed = 9.0f;
                jumpForce = 16.5f;
                break;
            case 70:
                this.gameObject.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                moveSpeed = 7.5f;
                jumpForce = 15.0f;
                break;
            case 90:
                this.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                moveSpeed = 6.5f;
                jumpForce = 12.5f;
                break;
            case 100:
                this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                moveSpeed = 5.0f;
                jumpForce = 10.0f;
                break;
        }
    }

    private void FixedUpdate()
    {
        // 플레이어 체력 확인 및 회복
        if (hp > 0 && hp < 100)
        {
            hp += 1 * Time.deltaTime;
        }
    }

    #region Methods
    


    private void MoveCharacter()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0; // 이동 입력 판별(True면 이동, False면 정지)

        // 점프 판별
        if (isGrounded)  // 점프중이 아님
        {
            // 에니매이션 처리
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))          // 앞으로, 왼쪽, 오른쪽으로 걷기
            {
                anim.SetInteger("WalkParam", 1);
                isWalking = true;
            }

            else if (Input.GetKey(KeyCode.S))           // 뒤로가기
            {
                anim.SetInteger("WalkParam", 2);
                isWalking = true;
            }

            else // Idle
            { 
                anim.SetInteger("WalkParam", 0);
                isWalking = false;
            }

            anim.SetInteger("JumpParam", 0);
            anim.SetBool("UsingJetpack", false);

            if (Input.GetKeyDown(KeyCode.Space)) // 점프
            {
                isWalking = false;
                isGrounded = false;
                anim.SetInteger("JumpParam", 1);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        else // 점프 또는 낙하 중
        {
            isWalking = false;
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
        UsingJetpack();

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

    // 제트팩 연산 및 애니메이션 처리
    private void UsingJetpack()
    {
        switch (isJetpackOn)
        {
            case true:
                anim.SetBool("UsingJetpack", true);                         // 제트팩 사용 애니메이션 실행
                particle.Play();                                            // 불꽃 파티클 실행
                fuel -= Time.deltaTime * 1.5f;                                     // 연료 사용
                rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Force);   // 제트팩 작동(상승) 부분
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
        // B3 - 2 입장
        if(other.CompareTag("PanelRoom") && PanelPuzzleController.level == 1)
        {
            GameObject.Find("Canvas").SetActive(false);
            SceneLoader.Instance.LoadScene("3.PanelPuzzle");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            rb.AddForce(Vector3.up * jumpForce * 2.5f, ForceMode.Force);
        }
        else if (other.CompareTag("FoodRoom"))
        {
            weight += Time.deltaTime * 1;
            if (weight > maxWeight)
                weight = maxWeight;

            ResizingWeight(weight);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 덤벨 획득 시 감량 하한 체중 재설정
        if(collision.gameObject.CompareTag("Dumbbell"))
        {
            dumCounts += 1; // 획득한 덤벨 개수 추가
            minWeight -= 10 * dumCounts;  // 체중 하한 재설정
            Debug.Log("Minimum weights: " + minWeight + "Kg");

            Destroy(collision.gameObject);
        }
        // 연료 획득 시 연료량 상한 재설정
        else if (collision.gameObject.CompareTag("Fuel"))
        {
            Destroy(collision.gameObject);
            maxFuel += 5.0f;
        }
        // 덤벨 획득 시 감량 하한 체중 재설정
        else if (collision.gameObject.CompareTag("Food"))
        {
            weight = maxWeight;
            this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Slime"))
        {
            rb.AddForce(-transform.forward * 1.5f, ForceMode.Impulse);
            hp -= 10;
            StartCoroutine("SetImmortalTimer");
        }
    }

    // 무적 타이머 작동 코루틴 
    IEnumerator SetImmortalTimer()
    {
        if(!isImmortal)
            yield return null;

        float timer = 0;
        isImmortal = true;

        while (timer<3)
        {
            float flick = Mathf.Abs(Mathf.Sin(Time.time * 100));    // 색 점멸, 0과 1을 반복적으로 반환
            body.materials[1].color = originColor * flick;
            yield return new WaitForSeconds(0.05f);
            timer += 0.05f;
        }

        body.materials[1].color = originColor;
        isImmortal = false;
        yield return null;
    }

    private void GrabObject()
    {
        if (isGrabbing)
        {
            Vector3 pos = transform.position + transform.forward * 5.0f + transform.up * 5.0f;
            grab.transform.position = pos;
        }
    }
}
#endregion