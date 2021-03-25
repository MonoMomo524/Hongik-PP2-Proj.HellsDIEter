using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public int id { get; set; }
    public string sentence { get; set; }
}

public class NPCSentence : MonoBehaviour
{
    public int id { get; set; }
    public int count { get; set; }
    public int level { get; set; }
    string[] line;
    List<string> sentences = new List<string>();
    private Dictionary<int, string> TableIndexDic = new Dictionary<int, string>();
    
    private void Start()
    {
        id = 1;
        level = 1;
        SetTable();
    }

    public void SetTable()
    {
        // csv 호출 및 불러오기
        TextAsset text = Resources.Load<TextAsset>("Dialogues");
        string content = text.text;

        // 행단위 구분
        line = content.Split('\n');

        for (int i = 2; i < line.Length; i++)
        {
            // 0 ~ 1번 row는 사용하지 않음
            string[] column = line[i].Split(',');
            int index = 0;
            Dialogue table = new Dialogue();
            table.id = int.Parse(column[index++]);
            table.sentence = column[index++];
            

            TableIndexDic.Add(table.id, table.sentence);
        }
    }

    private void OnMouseDown()
    {
        foreach (var item in TableIndexDic)
        {
            if (count < 5 && level == 1)
            {
                sentences.Add(item.Value);
                if (count == 4)
                {
                    count++;
                    break;
                }
            }
            else if (count > 4 && count < 9 && level == 2)
            {
                sentences.Add(item.Value);
                if (count == 8)
                {
                    count++;
                    break;
                }
            }
            else if (count > 8 && count < 11 && level == 3)
            {
                sentences.Add(item.Value);
                if (count == 10)
                {
                    count = 11;
                    break;
                }
            }
            else if (count>10 && count<15&& level==4)
            {
                sentences.Add(item.Value);
                if (count == 14)
                {
                    count++;
                    break;
                }
            }
            count++;
        }
        DialogueManager.instance.Ondialogue(sentences.ToArray());
        sentences.Clear();
        count = 0;
    }
}
