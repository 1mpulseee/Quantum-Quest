using UnityEngine;
[CreateAssetMenu(fileName = "NewMessageSettings", menuName = "Message Settings", order = 51)]
public class MessageSettings : ScriptableObject
{
    [SerializeField] private float delayBetweenHalves = 0.05f;
    [SerializeField] private float delayBetweenCharacters = 0.03f;
    [SerializeField] private float smoothTime = 0.1f;

    [Space(25)]
    [SerializeField] private bool isPaused = false;
    [SerializeField] private bool autoSkip = false;
    [SerializeField] private float skipDelay = 0.5f;

    [Space(25)]
    [SerializeField] private string[] messages;

    public float DelayBetweenHalves => delayBetweenHalves;
    public float DelayBetweenCharacters => delayBetweenCharacters;
    public float SmoothTime => smoothTime;
    public bool IsPaused => isPaused;
    public bool AutoSkip => autoSkip;
    public float SkipDelay => skipDelay;
    public string[] Messages => messages;
}