using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int ID;
    public string Name;
    public string Dis;

    private enum ITEM
    {
        DUMBEL,
        MAP,
        KEY,
        FOOD,
        TOOL
    }

    public Item(int id, string name, string dis)
    {
        ID = id;
        Name = name;
        Dis = dis;
    }
}

public class ItemManager : MonoBehaviour
{
    public List<Item> ItemList = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
