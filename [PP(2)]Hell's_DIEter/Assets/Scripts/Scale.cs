using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scale : MonoBehaviour
{
    public WeightPuzzleController Controller;
    public Transform[] Point = new Transform[3];
    public GameObject Slime;
    public GameObject Player;
    [SerializeField]private GameObject slime;
    private bool monster = false;
    private int count;

    void Start()
    {
        Point[0] = GameObject.Find("ScaleCatchPoint").transform;
        Point[1] = GameObject.Find("ScaleCatchPoint (1)").transform;
        Point[2] = GameObject.Find("ScaleCatchPoint (2)").transform;


        //Controller = GameObject.Find("Weight Puzzle Stage").GetComponent<WeightPuzzleController>();
        if (Controller == null)
            Debug.LogError("NULL");

            count = 0;
        //for (int i = 0; i < Controller.GoalSlimes; i++)
        //{
        //    InstanciatingMonsters(Slime);
        //}
    }

    public void CheckMonster()
    {
        if (monster == false)
        {
            return;
        }

        if (Controller.GoalSlimes > 0
            && Player.GetComponent<Player>().IsGrabbing == true
            && slime.GetComponent<Slime>().Estate == global::Slime.STATE.CATCHED)
        {
            Debug.Log(count);
            slime.transform.parent = null;
            slime.GetComponent<Slime>().Estate = global::Slime.STATE.ISOLATED;
            slime.GetComponent<Slime>().agent.Warp(Point[count].transform.position);
            slime.gameObject.GetComponent<SphereCollider>().enabled = false;
            Controller.GoalSlimes--;
            count++;
            slime = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Slime"))
        {
            slime = other.transform.gameObject;
            monster = true;
        }
        else
        {
            monster = false;
        }
    }

    void InstanciatingMonsters(GameObject mon)
    {
        Slime slime = mon.GetComponent<Slime>();
        slime.Estate = global::Slime.STATE.ROAMING;
        slime.points = new Transform[1];
        slime.points[0] = Point[0];

        Instantiate(slime, slime.points[0]);

        Debug.Log("DONE");
        return;
    }
}
