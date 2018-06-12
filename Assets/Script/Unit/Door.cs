using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer DoorSprite;
    public CommonData.NOTE_POS_TYPE NoteType;

    void Update()
    {
        // PC 에디터용
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                // 문을 선택 했다.
                if(hit.transform.name == this.name)
                    NoteManager.Instance.DeleteCheckNote(this);
                Debug.Log("Complete" + hit.collider.name);
            }
        }

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
