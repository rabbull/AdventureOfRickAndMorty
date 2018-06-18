using UnityEngine;

public abstract class Button : MonoBehaviour
{
    private GameObject _tmpobj;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            if (_tmpobj != null)
                _tmpobj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (!Input.GetMouseButtonDown(0)) return;
        var col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (col.Length <= 0) return;
        foreach (var c in col)
        {
            if (c.gameObject != gameObject) continue;
            _tmpobj = c.gameObject;
            c.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;
            OnClickEvent();
        }
    }

    protected abstract void OnClickEvent();
}
