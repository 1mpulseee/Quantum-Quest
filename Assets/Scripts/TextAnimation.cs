using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class TextAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] float betweenHalf = .05f;
    [SerializeField] float betweenChar = .03f;
    [SerializeField] float smoothTime = .1f;

    List<float> leftAplhas;
    List<float> rightAplhas;

    bool isAnimated;

    private void Start()
    {
        message.ForceMeshUpdate();
        leftAplhas = new float[message.text.Length].ToList();
        rightAplhas = new float[message.text.Length].ToList();

        Visible(false);
        isAnimated = true;
        StartCoroutine(Smooth(0));
    }
    private void Update()
    {
        if (isAnimated)
            SwitchColor();

        if (Input.GetKeyDown(KeyCode.X))
        {
            Visible(true);
            isAnimated = false;
        }
    }
    void Visible(bool visible)
    {
        StopAllCoroutines();
        DOTween.Kill(1);

        for (int i = 0; i < leftAplhas.Count; i++)
        {
            leftAplhas[i] = visible ? 255 : 0;
            rightAplhas[i] = visible ? 255 : 0;
        }
        SwitchColor();
    }

    void SwitchColor()
    {
        for (int i = 0; i < leftAplhas.Count; i++)
        {
            if (message.textInfo.characterInfo[i].character != '\n' && 
                message.textInfo.characterInfo[i].character != ' ')
            {
                int meshIndex = message.textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = message.textInfo.characterInfo[i].vertexIndex;

                Color32[] vertexColors = message.textInfo.meshInfo[meshIndex].colors32;

                vertexColors[vertexIndex + 0].a = (byte)leftAplhas[i];
                vertexColors[vertexIndex + 1].a = (byte)leftAplhas[i];
                vertexColors[vertexIndex + 2].a = (byte)rightAplhas[i];
                vertexColors[vertexIndex + 3].a = (byte)rightAplhas[i];
            }
        }
        message.UpdateVertexData();
    }
    IEnumerator Smooth(int i)
    {
        if (i >= leftAplhas.Count)
        {
            isAnimated = false;
            yield break;
        }
            

        DOTween.To(
            () => leftAplhas[i],
            x => leftAplhas[i] = x,
            255,
            smoothTime)
            .SetEase(Ease.Linear)
            .SetId(1);

        yield return new WaitForSecondsRealtime(betweenHalf);

        DOTween.To(
            () => rightAplhas[i],
            x => rightAplhas[i] = x,
            255,
            smoothTime)
            .SetEase(Ease.Linear)
            .SetId(1);

        yield return new WaitForSecondsRealtime(betweenChar);

        StartCoroutine(Smooth(i + 1));
    }
}