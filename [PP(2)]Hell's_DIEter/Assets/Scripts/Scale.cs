using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scale : MonoBehaviour
{
    public WeightPuzzleController Controller;
    public Transform[] Point = new Transform[3];
    public GameObject Slime;
    public Player Player;
    [SerializeField]private GameObject slime;
    private bool monster = false;
    private int count;

    void Start()
    {
        Point[0] = GameObject.Find("ScaleCatchPoint").transform;
        Point[1] = GameObject.Find("ScaleCatchPoint (1)").transform;
        Point[2] = GameObject.Find("ScaleCatchPoint (2)").transform;

        Player = GameObject.Find("Player").GetComponent<Player>();

        count = 0;
        //for (int i = 0; i < Controller.GoalSlimes; i++)
        //{
        //    InstanciatingMonsters(Slime);
        //}
    }

    public void CheckMonster()
    {
        if (monster == false || Controller==null)
            return;

        if (Controller.GoalSlimes > 0
            && Player.IsGrabbing == true
            && slime.GetComponent<Slime>().Estate == global::Slime.STATE.CATCHED)
        {
            slime.transform.parent = null;
            slime.transform.position = Point[count].position;
            slime.GetComponent<Slime>().Estate = global::Slime.STATE.ISOLATED;
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

        return;
    }
}
