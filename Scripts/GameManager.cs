using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isTouched;
    public SpriteRenderer pic;

    public GameObject progressor;
    public RectTransform processor;

    public Texture2D[] pics = new Texture2D[8];

    // Start is called before the first frame update
    void Start()
    {
        isTouched = false;

        GameObject canvas = GameObject.Find("Canvas");

        GameObject pb = Instantiate(progressor, canvas.transform);
        pb.GetComponent<RectTransform>().transform.position = new Vector3(0, -Screen.height * 0.5f + 20, 0) + canvas.transform.position;
        pb.GetComponent<RectTransform>().transform.localScale = new Vector3(0.575f, 0.575f, 0.575f);
        processor = (RectTransform)pb.GetComponent<RectTransform>().Find("Progress_Upper");

        //openingStart();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0) || (Input.GetMouseButtonDown(0)) && !isTouched)
        {
            isTouched = true;
            StopCoroutine("opening");
            pic.GetComponent<WhiteScreen>().FadeOut(1.0f, () => { });
            StartCoroutine(AudioFadeOut(this.GetComponent<AudioSource>(), 1.0f));
        }
    }

    public void openingStart()
    {
        StartCoroutine("opening");
    }

    IEnumerator opening()
    {
        processor.GetComponent<ProgressBar>().SetDirection(0);
        processor.GetComponent<ProgressBar>().setPercent(0);

        for (int i = 0; i < 7; i++)
        {
            Rect rect = new Rect(0, 0, pics[i].width, pics[i].height);
            pic.sprite = Sprite.Create(pics[i], rect, new Vector2(0.5f, 0.5f));//Resources.Load<Sprite>("op_" + i);//Assets/Resources/op_1.jpg
            pic.GetComponent<WhiteScreen>().SetScale();
            if (i == 6)
                pic.color = new Color(1, 1, 1, 1);
            else
            {
                pic.color = new Color(1, 1, 1, 0);
                pic.GetComponent<WhiteScreen>().FadeIn(1.0f, () => {
                    pic.GetComponent<WhiteScreen>().FadeTo(2.0f, 1.0f, () => {
                        pic.GetComponent<WhiteScreen>().FadeOut(1.0f, () => {
                        });
                    });
                });
            }
            processor.GetComponent<ProgressBar>().setPercent((i + 1) * 100 / 7, 0.5f);

            yield return new WaitForSeconds(4.0f);
        }

        Debug.Log("opening end");
        StartCoroutine(AudioFadeOut(this.GetComponent<AudioSource>(), 1.0f));
    }

    IEnumerator AudioFadeOut(AudioSource audio, float fadeTime)
    {
        float startVol = audio.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime / fadeTime)
        {
            audio.volume = Mathf.Lerp(startVol, 0.0f, t);
            yield return null;
        }
        audio.Stop();

        SceneManager.LoadScene("MainScene");
    }
}
