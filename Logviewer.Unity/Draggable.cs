using UnityEngine;
using UnityEngine.EventSystems;

namespace Logviewer.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public class Draggable : MonoBehaviour, IDragHandler
    {
        private RectTransform _panelTransform;

        public void Start()
        {
            _panelTransform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData data)
        {
            // Move gameobject the same amount as the mouse moved
            _panelTransform.position += new Vector3(data.delta.x, data.delta.y, 0);

            // If the gameobject is now beyond one of the four edges of the screen, move it back by the amount it is beyond the edge
            // Note: I think this assumes a certain pivot, but it works with the Rail Route UI canvasses
            Vector2 lowerLeft = new Vector2(
                _panelTransform.position.x + _panelTransform.rect.x,
                _panelTransform.position.y + _panelTransform.rect.y
            );
            Vector2 upperRight = new Vector2(
                _panelTransform.position.x + _panelTransform.rect.x + _panelTransform.rect.width,
                _panelTransform.position.y + _panelTransform.rect.y + _panelTransform.rect.height
            );

            if (lowerLeft.x < 0)
                _panelTransform.position -= new Vector3(lowerLeft.x, 0, 0);
            else if (upperRight.x > Screen.currentResolution.width)
                _panelTransform.position -= new Vector3(upperRight.x - Screen.currentResolution.width, 0, 0);

            if (_panelTransform.position.y + _panelTransform.rect.y < 0)
                _panelTransform.position -= new Vector3(0, lowerLeft.y, 0);
            else if (upperRight.y > Screen.currentResolution.height)
                _panelTransform.position -= new Vector3(0, upperRight.y - Screen.currentResolution.height, 0);
        }
    }
}
