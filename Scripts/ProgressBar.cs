using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    enum Direction
    {
        HORIZONTAL,
        VERTICAL
    }
    RectTransform rt;
    float maxWidth, maxHeight;
    ProgressBar.Direction direction;
    private void Awake()
    {
        rt = this.GetComponent<RectTransform>();
        maxWidth = this.GetComponent<RectTransform>().rect.width;
        maxHeight = this.GetComponent<RectTransform>().rect.height;

        Debug.Log("PB Awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PB Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirection(int dir)
    {
        direction = Direction.HORIZONTAL + dir;
    }
    public void setPercent(float percent)//percent = 0~100
    {
        if (direction == Direction.HORIZONTAL)
            rt.sizeDelta = new Vector2(percent * maxWidth / 100.0f, rt.sizeDelta.y);
        else
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, percent * maxHeight / 100.0f);
    }
    public void setPercent(float percent, float prcTime)//percent = 0~100
    {
        StartCoroutine(progressing(percent, prcTime));
    }
    IEnumerator progressing(float prcPercent, float prcTime)
    {
        if (!rt)
            yield return null;

        float value;

        if (prcTime <= 0)
        {
            if (direction == Direction.HORIZONTAL)
                rt.sizeDelta = new Vector2(prcPercent * maxWidth / 100.0f, rt.sizeDelta.y);// rect.Set(rt.rect.x, rt.rect.y, prcPercent * maxWidth / 100.0f, rt.rect.height);
            else
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, prcPercent * maxWidth / 100.0f);// rect.Set(rt.rect.x, rt.rect.y, rt.rect.width, prcPercent * maxHeight / 100.0f);
            yield return null;
        }
        else
        {
            if (direction == Direction.HORIZONTAL)
                value = rt.rect.width;//sizeDelta.x;
            else
                value = rt.rect.height;//sizeDelta.y;
            for (float t = 0; t < 1.0f; t += Time.deltaTime / prcTime)
            {
                float percent;
                if (direction == Direction.HORIZONTAL)
                    percent = Mathf.Lerp(value, prcPercent * maxWidth / 100.0f, t);
                else
                    percent = Mathf.Lerp(value, prcPercent * maxHeight / 100.0f, t);

                if (direction == Direction.HORIZONTAL)
                    rt.sizeDelta = new Vector2(percent, rt.sizeDelta.y);
                else
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, percent);

                yield return null;
            }
        }
    }
}
