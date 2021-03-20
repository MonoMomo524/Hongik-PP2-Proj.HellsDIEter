using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Sprite[] buttons;    // 버튼 스프라이트 저장
    bool isOpen = false;          // 인벤토리 창이 열려있는지

    // 인벤토리 창 열기/닫기
    public void Inventory()
    {
        if (isOpen == true)
        {
            RectTransform inventory = GameObject.Find("Inventory").GetComponent<RectTransform>();
            inventory.pivot = new Vector2(0.5f, 0.85f);
            inventory.position = Vector3.Lerp(inventory.position, new Vector3(inventory.position.x, 0.0f), 3.0f);

            this.GetComponent<Image>().sprite = buttons[0];
            isOpen = false;
        }
        else
        {
            RectTransform inventory = GameObject.Find("Inventory").GetComponent<RectTransform>();
            inventory.pivot = new Vector2(0.5f, 0.0f);
            inventory.position = Vector3.Lerp(inventory.position, new Vector3(inventory.position.x, 0.0f), 3.0f);

            this.GetComponent<Image>().sprite = buttons[1];
            isOpen = true;
        }

        return;
    }
}
