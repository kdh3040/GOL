using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorData Data;
    public SpriteRenderer DoorSprite;
    public CommonData.NOTE_LINE NoteLineType;

    public void SetData(int id, CommonData.NOTE_LINE type)
    {
        Data = DataManager.Instance.DoorDataDic[id];
        DoorSprite.sprite = (Sprite)Resources.Load(Data.img, typeof(Sprite));
        NoteLineType = type;
    }

    void Update()
    {
        // PC 에디터용

        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == this.name)
                    GamePlayManager.Instance.ClickDoor(this);
                Debug.Log("Complete" + hit.collider.name);
            }
            else
            {
                Debug.Log("null");
            }
        }
    */

        /*
        // 모바일용
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if(Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                if(hit)
                {
                    Debug.Log("asdfasdf");
                }
            }
        }*/
    }
}
