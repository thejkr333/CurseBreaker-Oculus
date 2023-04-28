using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgressBar : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private Camera camera;

    private Image progress;

    // Start is called before the first frame update
    void Awake()
    {
        camera = FindObjectOfType<Camera>();
        progress = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.frameCount > 0)
            progress.fillAmount = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        trySkip(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        trySkip(eventData);
    }

    private void trySkip(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(progress.rectTransform, eventData.position, camera, out Vector2 localPoint))
        {
            float pct = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);
            skipToPercent(pct);
        }
    }

    private void skipToPercent(float pct)
    {
        var frame = videoPlayer.frameCount * pct;
        videoPlayer.frame = (long)frame;
    }
}
