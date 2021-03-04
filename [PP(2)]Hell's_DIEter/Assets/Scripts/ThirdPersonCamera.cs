using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private GameObject player;

    private float rotSpeed = 2.0f;

    private float camDistance = 0;      // 리그부터 카메라까지의 거리
    private float camWidth = -10.0f;    // 가로 거리
    private float camHeight = 4.0f;     // 세로 거리
    private float camFix = 3.0f;        // 레이캐스트 후 리그를 향해 올 보정 값(거리)

    Vector3 direction;
    Vector3 mouseMove;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // 카메라 리그부터 카메라까지의 거리
        camDistance = Mathf.Sqrt(camWidth * camWidth + camHeight * camHeight);

        // 카메라 리그부터 카메라 위치까지의 방향벡터
        direction = new Vector3(0, camHeight, camWidth).normalized;
    }

    private void Update()
    {
        LookAround();
    }

    private void LookAround()
    {
        // 카메라 회전 관련
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed, Space.World);
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed, Space.Self);
        transform.position = player.transform.position;

        transform.Translate(player.transform.position);
        Debug.DrawRay(transform.position, new Vector3(transform.forward.x, 0f, transform.forward.z).normalized, Color.red);
    }
}
