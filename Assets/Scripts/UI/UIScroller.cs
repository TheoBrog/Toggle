using UnityEngine;
using UnityEngine.UI;

public class UIScroller : MonoBehaviour
{
    public RawImage background;
    public float scrollSpeed = 0.2f;

    void Update()
    {
        // move a textura no eixo Y
        Rect uv = background.uvRect;
        uv.y += scrollSpeed * Time.deltaTime;
        background.uvRect = uv;
    }
}