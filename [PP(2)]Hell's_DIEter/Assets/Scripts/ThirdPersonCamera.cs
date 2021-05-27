using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private GameObject player;

    private float rotSpeed = 5.0f;

    private float camDistance = 0;      // 리그부터 카메라까지의 거리
    private float camWidth = -20.0f;    // 가로 거리
    private float camHeight = 5.0f;     // 세로 거리
    private float camFix = 3.0f;        // 레이캐스트 후 리그를 향해 올 보정 값(거리)

    Vector3 direction;
    Vector3 mouseMove;
    Vector3 stopMouse;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // 카메라 리그부터 카메라까지의 거리
        camDistance = Mathf.Sqrt(camWidth * camWidth + camHeight * camHeight);

        // 카메라 리그부터 카메라 위치까지의 방향벡터
        direction = new Vector3(0, camHeight, camWidth).normalized;

        // 마우스 현재 위치 감지
        stopMouse = Input.mousePosition;
    }

    private void Update()
    {
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
    }

    private void LookAround()
    {
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
    }
}
