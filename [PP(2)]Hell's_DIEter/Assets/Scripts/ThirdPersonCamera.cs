using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private GameObject player;
    private GameObject mainCamera;

    private float rotSpeed = 50.0f;

    private float camDistance = 0;      // 리그부터 카메라까지의 거리
    private float camWidth = -10.0f;    // 가로 거리
    private float camHeight = 4.0f;     // 세로 거리
    private float camFix = 3.0f;        // 레이캐스트 후 리그를 향해 올 보정 값(거리)

    Vector3 direction;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        // Length from camera rig to camera.
        camDistance = Mathf.Sqrt(camWidth * camWidth + camHeight * camHeight);

        // Direction vector from camera rig to camera position.
        direction = new Vector3(0, camHeight, camWidth).normalized;
    }

    private void Update()
    {
        // Rotation from y axsis.
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed, Space.World);
        // Rotation from x axsis.
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed, Space.Self);

        transform.position = player.transform.position;

        // Vector value for ray casting
        Vector3 rayTarget = transform.up * camHeight + transform.forward * camWidth;

        RaycastHit hitInfo;
        Physics.Raycast(transform.position, rayTarget, out hitInfo, camDistance);

        // If ray casting was successful
        if (hitInfo.point != Vector3.zero)
        {
            // Move camera to point.
            mainCamera.transform.position = hitInfo.point;

            // Fix the camera.
            mainCamera.transform.Translate(direction * -1 * camFix);
        }
        else
        {
            // Set the local position to zero. (Move the camera rig.)
            mainCamera.transform.localPosition = Vector3.zero;
            // Translate the camera.
            mainCamera.transform.Translate(direction * camDistance);

            // Fix the camera.
            mainCamera.transform.Translate(direction * -1 * camFix);
        }

        transform.Translate(player.transform.position);
    }
}
