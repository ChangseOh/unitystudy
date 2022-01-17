using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteScreen : MonoBehaviour
{
    public delegate void Callback();
    public bool mode;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<SpriteRenderer>().sprite != null)
            SetScale();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetScale()
    {
        Camera maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        maincamera.orthographicSize = Screen.height / 2 * 0.01f;
        Sprite spr = this.GetComponent<SpriteRenderer>().sprite;
        float scale;
        if (mode)
            scale = Mathf.Max(Screen.width / spr.rect.width, Screen.height / spr.rect.height);
        else
            scale = Mathf.Min(Screen.width / spr.rect.width, Screen.height / spr.rect.height);
        this.transform.localScale = new Vector3(scale, scale, 1.0f);//.Set(scale, scale, scale);

    }

    public void FadeIn(float dt, Callback _func)
    {
        StartCoroutine(_FadeTo(dt, 1.0f, _func));
    }
    public void FadeOut(float dt, Callback _func)
    {
        StartCoroutine(_FadeTo(dt, 0.0f, _func));
    }
    public void FadeTo(float dt, float dest, Callback _func)
    {
        StartCoroutine(_FadeTo(dt, dest, _func));
    }
    public IEnumerator _FadeTo(float dt, float dest, Callback _func)
    {
        float alpha = this.transform.GetComponent<SpriteRenderer>().color.a;//material.color.a;
        if (dt <= 0)
        {
            this.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, dest);
            yield return null;
        }
        else
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / dt)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, dest, t));
                this.transform.GetComponent<SpriteRenderer>().color = newColor;
                yield return null;
            }
        }
        _func();
    }
}
