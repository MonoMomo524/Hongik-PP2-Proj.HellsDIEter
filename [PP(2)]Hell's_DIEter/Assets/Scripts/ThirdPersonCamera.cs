using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private GameObject player;

<<<<<<< Updated upstream
    private float rotSpeed = 5.0f;

    private float camDistance = 0;      // 리그부터 카메라까지의 거리
    private float camWidth = -20.0f;    // 가로 거리
    private float camHeight = 5.0f;     // 세로 거리
    private float camFix = 3.0f;        // 레이캐스트 후 리그를 향해 올 보정 값(거리)

    Vector3 direction;
    Vector3 mouseMove;
    Vector3 stopMouse;
=======
    float camDistance = 0;      // 리그부터 카메라까지의 거리
    float camWidth = -20.0f;    // 가로 거리
    float camHeight = 5.0f;     // 세로 거리
    float camFix = 3.0f;        // 레이캐스트 후 리그를 향해 올 보정 값(거리)
    float distance = 20f;
    float minZoom = 10.0f;       //줌 인했을 대 최소 거리
    float maxZoom = 30.0f; // 줌아웃했을 때 최대 거리
    float sensitivity = 100f; // 마우스 감도
    float zoomSpeed = 7f; // 마우스 감도
    Vector3 direction;

    // 2021 하계 유니티 기초 학술회에서 구현했던 TPS Camera로 Update
    float x;
    float y;

    GameObject transparentObj;  // 반투명화 될 오브젝트
    Renderer ObstacleRenderer;  // 오브젝트를 반투명하게 만들어주는 렌더러
    List<GameObject> Obstacles; // 반투명화 된 장애물 리스트
>>>>>>> Stashed changes

    private void Start()
    {
        // 마우스 상태 설정
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 플레이어 태그를 가진 게임오브젝트(=플레이어)를 찾아서 넣기
        player = GameObject.FindObjectOfType<Player>().gameObject;
        Obstacles = new List<GameObject>(); // 새 리스트 생성

        // 기준점으로부터 카메라까지의 길이
        camDistance = Mathf.Sqrt(camWidth * camWidth + camHeight * camHeight);

        // 기준점으로부터 카메라 위치까지의 방향
        direction = new Vector3(0, camHeight, camWidth).normalized;

        // 마우스 현재 위치 감지
        stopMouse = Input.mousePosition;
    }

    private void Update()
    {
<<<<<<< Updated upstream
        ////y축 기준 회전
        //transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed, Space.World);
        ////x축 기준 회전
        //transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed, Space.Self);

        //transform.position = player.transform.position;
        ////레이캐스트할 벡터값
        //Vector3 ray_target = transform.up * camHeight + transform.forward * camWidth;

        //RaycastHit hitinfo;
        //Physics.Raycast(transform.position, ray_target, out hitinfo, camDistance);

        //if (hitinfo.point != Vector3.zero)//레이케스트 성공시
        //{
        //    //point로 옮긴다.
        //    this.transform.position = hitinfo.point;
        //    //카메라 보정
        //    this.transform.Translate(direction * -1 * camFix);
        //}
        //else
        //{
        //    //로컬좌표를 0으로 
        //    this.transform.localPosition = Vector3.zero;
        //    //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
        //    this.transform.Translate(direction * camDistance);

        //    //카메라 보정
        //    this.transform.Translate(direction * -1 * camFix);
        //}

        LookAround();
=======
        // 마우스 On/Off
        if (Input.GetKeyDown(KeyCode.LeftControl))
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

        if (Cursor.visible == true)
            return;

        RotateAround();
        CalculateZoom();
        FadeOut();
    }

    private void RotateAround()
    {
        // 마우스의 위치를 받아오기
        x += Input.GetAxis("Mouse X") * sensitivity * 0.01f; // 마우스 좌우 움직임 감지
        y -= Input.GetAxis("Mouse Y") * sensitivity * 0.01f; // 마우스 상하 움직임 감지

        // 카메라 높이값(끄덕끄덕각도) 제어
        if (y < 0)  // 바닥을 뚫지 않게
            y = 0;
        if (y > 50) // Top View(정수리로 내려보기)로 하고 싶다면 90으로 바꾸기
            y = 50;

        // player.transform을 자주 사용할건데 너무 길어서 치환 => target
        Transform target = player.transform;

        // 카메라가 회전할 각도와 이동할 위치 계산
        Quaternion angle = Quaternion.Euler(y, x, 0);
        Vector3 destination = angle * (Vector3.back * distance) + target.position + Vector3.zero;

        transform.rotation = angle;             // 카메라 각도 조정
        transform.position = destination;   // 카메라 위치 조정

        //레이캐스트할 벡터값
        Vector3 ray_target = transform.up * camHeight + transform.forward * camWidth;

        RaycastHit hitinfo;
        Physics.Raycast(transform.position, ray_target, out hitinfo, camDistance);

        if (hitinfo.point != Vector3.zero)//레이케스트 성공시
        {
            //point로 옮긴다.
            transform.position = hitinfo.point;
            //카메라 보정
            transform.Translate(direction * -1 * camFix);
        }
        else
        {
            //로컬좌표를 0으로 맞춘다. (카메라리그로 옮긴다.)
            transform.localPosition = Vector3.zero;
            //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
            transform.Translate(direction * camDistance);
            //카메라 보정
            transform.Translate(direction * -1 * camFix);
        }
    }

    // 카메라 확대율 계산
    void CalculateZoom()
    {
        // 마우스 줌 인/아웃
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        // 줌 최소/최대 제한
        // Clamp함수 : 최대/최소값을 설정해주고 제한
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    // 최하단 플로어가 아닌 게임오브젝트가 플레이어를 가리지 못하도록 반투명화 하는 메소드
    private void FadeOut()
    {
        // Raycast를 이용하여 플레이어와 카메라 사이에 있는 오브젝트 감지
        // 오브젝트로 감지되지 않으려면 Layer를 Ignor Raycast로 바꿔놓아야 함
        // Ignore Raycast: Player, Ground, Particles
        float distance = Vector3.Distance(transform.position, player.transform.position) - 1;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits;

        // 카메라에서 플레이어를 향해 레이저를 쏘았을 때 맞은 오브젝트가 있다면
        hits = Physics.RaycastAll(transform.position, direction, distance);

        bool remove = true;
        if (Obstacles.Count != 0 && hits != null)
        {
            for (int i = 0; i < Obstacles.Count; i++)
            {
                foreach (var hit in hits)
                {
                    // hit된 오브젝트가 리스트에 저장되지 않았은 것이면 계속 탐색
                    if (Obstacles[i] != hit.collider.gameObject)
                        continue;
                    // 저장된 오브젝트면 유지
                    else
                    {
                        remove = false;
                        break;
                    }
                }

                // 삭제 대상이면
                if (remove == true)
                {
                    ObstacleRenderer = Obstacles[i].GetComponent<MeshRenderer>();
                    RestoreMaterial();

                    Obstacles.Remove(Obstacles[i]);
                }
            }
        }

        if (hits.Length > 0)
        {
            // 이미 저장된 오브젝트인지 확인
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.DrawRay(transform.position, direction * distance, Color.red);

                transparentObj = hits[i].collider.gameObject;

                // 장애물 레이어가 없다면 다음 오브젝트 검사
                if (transparentObj.layer != 13)
                    continue;

                // 이미 저장된 오브젝트이면 다음 오브젝트 검사
                if (Obstacles != null && Obstacles.Contains(transparentObj))
                    continue;

                // 저장되지 않은 오브젝트면 투명화 후 리스트에 추가
                if (transparentObj.layer == 9)
                    ObstacleRenderer = transparentObj.GetComponent<Renderer>();
                if (ObstacleRenderer != null && transparentObj != null)
                {
                    // 오브젝트를 반투명하게 렌더링한다
                    Material material = ObstacleRenderer.material;
                    Color matColor = material.color;
                    matColor.a = 0.5f;
                    material.color = matColor;

                    // 리스트에 추가
                    Obstacles.Add(transparentObj);
                    ObstacleRenderer = null;
                    transparentObj = null;
                }
            }
        }
>>>>>>> Stashed changes
    }

    // 기존 투명화한 오브젝트를 원상복구 하는 메소드
    void RestoreMaterial()
    {
<<<<<<< Updated upstream
        // 카메라 회전 관련
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.fixedDeltaTime * rotSpeed, Space.World);
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * rotSpeed, Space.Self);
        transform.position = player.transform.position;

        transform.Translate(player.transform.position);
    }

    public void SetDistance(int weight)
    {
        switch (weight)
        {
            case 0:
                break;
            case 40:
            case 70:
                camWidth = -10.0f;    // 가로 거리
                camHeight = 4.0f;     // 세로 거리
                break;
            case 90:
                camWidth = -20.0f;    // 가로 거리
                camHeight = 5.0f;     // 세로 거리
                break;
        }
=======
        Material material = ObstacleRenderer.material;
        Color matColor = material.color;
        matColor.a = 1f;    // 알파값 1:불투명(원상복구)
        material.color = matColor;

        ObstacleRenderer = null;
>>>>>>> Stashed changes
    }
}
