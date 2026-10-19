using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class TargetingReticleController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The RectTransform that defines the screen-space area used to position the targeting reticule.")]
    [SerializeField]
    private RectTransform canvasRectTransform;

    [SerializeField] 
    private Transform aimOrigin;

    [Tooltip("The camera used to represent the player's current aim position.")] 
    [SerializeField] 
    private Camera gameplayCamera;
    

    [Tooltip("The RectTransform moved to represent the player's current aim position.")]
    [SerializeField]
    private RectTransform targetingReticle;

    [Header("Gamepad Input")] 
    private float gamepadReticleSpeed = 800f;

    
    [Range(0f, 1f)]
    [SerializeField]
    private float gamepadDeadzone = 0.15f;

    [Header("World-Space Aiming")] 
    
    [SerializeField]
    private float aimDistance = 100f;

    private Vector3 worldAimPoint;
    
    // Update is called once per frame
    void Update()
    {
        if (!MoveReticleWithGamepad())
        {
            MoveReticleWithMouse();
        }

        UpdateWorldAimPoint();
    }

    public Vector3 GetAimDirection(Vector3 origin)
    {
        return (worldAimPoint - origin).normalized;
        
    }

    private void UpdateWorldAimPoint()
    {
        Vector2 reticleScreenPosition = RectTransformUtility.WorldToScreenPoint(cam: null, targetingReticle.position);
        Ray cameraRay = gameplayCamera.ScreenPointToRay(reticleScreenPosition);
        worldAimPoint = cameraRay.GetPoint(aimDistance);

        Debug.DrawRay(start: cameraRay.origin, dir: cameraRay.direction * aimDistance, Color.yellow);
        Vector3 aimDirection = GetAimDirection(aimOrigin.position);

        Debug.DrawLine(aimOrigin.position, worldAimPoint, Color.cyan);

    }

    private bool MoveReticleWithGamepad()
    {
        if (Gamepad.current == null)
        {
            return false;
        }

        Vector2 gamepadInput = Gamepad.current.rightStick.ReadValue();

        if (gamepadInput.magnitude < gamepadDeadzone)
        {
            return false;
        }

        Vector2 reticleMovement = gamepadInput * gamepadReticleSpeed * Time.deltaTime;
        Vector2 nextPosition = targetingReticle.anchoredPosition + reticleMovement;

        targetingReticle.anchoredPosition = ClampReticlePosition(nextPosition);

        return true;
    }

    private void MoveReticleWithMouse()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.delta.ReadValue().sqrMagnitude == 0f)
        {
            return;
        }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        bool founderCanvasPosition = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, mouseScreenPosition, cam:null, localPoint:out Vector2 mouseCanvasPosition);

        if (founderCanvasPosition == false)
        {
            return;
        }

        targetingReticle.anchoredPosition = ClampReticlePosition(mouseCanvasPosition);
    }

    private Vector2 ClampReticlePosition(Vector2 position)
    {
        Rect canvasRect = canvasRectTransform.rect;

        Vector2 reticleHalfSize = targetingReticle.rect.size * 0.5f;

        position.x = Mathf.Clamp(position.x, min: canvasRect.xMin + reticleHalfSize.x, max: canvasRect.xMax - reticleHalfSize.x);
        position.y = Mathf.Clamp(position.y, min: canvasRect.xMin + reticleHalfSize.y, max: canvasRect.yMax - reticleHalfSize.y);
        return position;
    }
}
