using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class TestMiniGame1 : MonoBehaviour
{
    [Header("Base Settings")]
    public RectTransform background;
    public RectTransform leftImage;
    public RectTransform rightImage;
    public RectTransform movingStrip;
    public Button checkButton;

    [Header("Gap Settings")]
    public float minGap = 50f;
    public float maxGap = 150f;

    [Header("Movement Settings")]
    public float baseSpeed = 100f;

    private float currentSpeed;
    private bool movingRight = true;
    private float gapStart;
    private float gapWidth;
    private float parentWidth;

    public UnityEvent onWin;
    public UnityEvent onLose;

    void Start()
    {
        parentWidth = background.rect.width;
        checkButton.onClick.AddListener(CheckPosition);
        GenerateRandomGap();
        InitializeMovement();
    }

    void Update()
    {
        UpdateMovement();
        CheckBounds();
    }

    void InitializeMovement()
    {
        currentSpeed = baseSpeed;
        movingStrip.anchoredPosition = new Vector2(0, movingStrip.anchoredPosition.y);
    }

    void UpdateMovement()
    {
        float direction = movingRight ? 1 : -1;
        float positionX = movingStrip.anchoredPosition.x;

        positionX += currentSpeed * Time.deltaTime * direction;

        movingStrip.anchoredPosition = new Vector2(positionX, movingStrip.anchoredPosition.y);
    }

    void CheckBounds()
    {
        float currentX = movingStrip.anchoredPosition.x;
        float stripWidth = movingStrip.rect.width;

        if (currentX + stripWidth > parentWidth && movingRight)
        {
            movingRight = false;
        }
        else if (currentX < 0 && !movingRight)
        {
            movingRight = true;
        }
    }

    public void GenerateRandomGap()
    {
        parentWidth = background.rect.width;

        gapWidth = Random.Range(minGap, maxGap);
        float maxStart = parentWidth - gapWidth;
        gapStart = Random.Range(0f, maxStart);

        leftImage.sizeDelta = new Vector2(gapStart, leftImage.sizeDelta.y);
        rightImage.sizeDelta = new Vector2(parentWidth - gapStart - gapWidth, rightImage.sizeDelta.y);
    }

    void CheckPosition()
    {
        float stripPosition = movingStrip.anchoredPosition.x;
        float stripWidth = movingStrip.rect.width;

        // –ассчитываем зону попадани€
        float safeZoneStart = gapStart;
        float safeZoneEnd = gapStart + gapWidth;

        // ѕровер€ем пересечение полоски с безопасной зоной
        bool isWin = stripPosition < safeZoneEnd && (stripPosition + stripWidth) > safeZoneStart;

        if (isWin)
        {
            Debug.Log("Win");
            onWin.Invoke();
        }
        else
        {
            Debug.Log("Lose");
            onLose.Invoke();
        }
    }
}