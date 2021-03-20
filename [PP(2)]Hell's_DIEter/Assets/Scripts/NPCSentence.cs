using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSentence : MonoBehaviour
{
    public string[] sentences;

    private void OnMouseDown()
    {
        DialogueManager.instance.Ondialogue(sentences);
    }
}
