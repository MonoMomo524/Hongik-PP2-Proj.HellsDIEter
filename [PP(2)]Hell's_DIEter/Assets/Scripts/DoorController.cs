using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public int limit;
    TextMeshProUGUI sentence;

    private void Start()
    {
        sentence = GameObject.Find("UI").transform.Find("Dialogue").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnDoorwayOpen(float weight)
    {
        if ((int)weight > limit)
        {
            StartCoroutine(ShowDoorInfo());
            return;
        }

        this.gameObject.SetActive(false);
    }

    private void OnDoorwayClose()
    {
        this.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            float weight = collision.transform.GetComponent<Player>().Weight;
            OnDoorwayOpen(weight);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnDoorwayClose();
        }
    }

    private void OnMouseDown()
    {
        OnDoorwayOpen(GameObject.FindObjectOfType<Player>().Weight);
    }

    IEnumerator ShowDoorInfo()
    {
        sentence.text = "여길 지나가고 싶거든 " + limit + "kg 이하로 감량해라!\n-앙마-";
        yield return new WaitForSeconds(4f);

        this.gameObject.SetActive(false);
    }
}
