using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float jumpPower;
    private AudioSource audio;
    public AudioClip jumpSound;

    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.jumpSound;
        this.audio.loop = false;    }

    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, jumpPower, 0);
            this.audio.Play();
            Debug.Log("JUMP");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        SceneManager.LoadScene("WallGame");
    }
}
