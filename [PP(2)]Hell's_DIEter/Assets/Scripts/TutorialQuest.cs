using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuest : MonoBehaviour
{
    private int id;
    bool done = false;
    private Player player;
    private NPCSentence npc;

    public GameObject dumb;
    public GameObject food;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        npc = GameObject.Find("Devil NPC").GetComponent<NPCSentence>();

        id = npc.count;
    }

    // Update is called once per frame
    void Update()
    {
        CheckQuest();
    }

    private void CheckQuest()
    {
        switch (npc.level)
        {
            case 1:
                if (player.IsJetpackOn)
                    done = true;
                break;
            case 2:
                if (player.Weight == 90)
                    done = true;
                break;
            case 3:
                if (player.Weight == 100)
                    done = true;
                break;
        }

        if (done)
        {
            if(npc.level==1)
            {
                Instantiate(dumb);
            }
            else if (npc.level == 2)
            {
                Instantiate(food);
            }

            npc.level += 1;
            done = false;
        }
    }
}
