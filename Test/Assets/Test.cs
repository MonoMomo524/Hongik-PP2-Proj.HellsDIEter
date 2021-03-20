using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Player
//{
//    private int hp = 100;
//    private int power = 50;

//    public void Attack()
//    {
//        Debug.Log(this.power + "damage");
//    }

//    public void Damage(int damage)
//    {
//        this.hp -= damage;
//        Debug.Log(damage + "damaged");
//    }
//}

public class Test : MonoBehaviour
{
    int[] tana = { 1, 2, 3, 4, 5 };

    // Start is called before the first frame update
    void Start()
    {
        //Player myPlayer = new Player();
        //myPlayer.Attack();
        //myPlayer.Damage(30);

        Vector2 playerPos = new Vector2(2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
