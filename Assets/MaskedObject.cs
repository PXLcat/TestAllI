using UnityEngine;
using UnityEngine.EventSystems;

public class MaskedObject : UIBehaviour
{
    [SerializeField]
    private CanvasRenderer canvasRendererToClip = null;

    private Canvas rootCanvas = null;
    private RectTransform maskRectTransform = null;
    private bool initialized = false;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (initialized)
        {
            SetTargetClippingRect();
        }
    }

    public void Initialize(Canvas rootCanvas, RectTransform maskRectTransform)
    {
        this.rootCanvas = rootCanvas;
        this.maskRectTransform = maskRectTransform;
        SetTargetClippingRect();
        initialized = true;
    }

    private void SetTargetClippingRect()
    {
        Rect rect = maskRectTransform.rect;
        // Get local position of maskRect as if it was direct child of root canvas, then offset mask rect by that amount
        rect.center += (Vector2)rootCanvas.transform.InverseTransformPoint(maskRectTransform.position);
        canvasRendererToClip.EnableRectClipping(rect);
    }
}
