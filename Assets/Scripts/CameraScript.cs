using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform BaseCam;
    public static bool isDialogOpen;

    private void Start()
    {
        BaseCam = transform;
    }

    private void Update()
    {
        if (isDialogOpen) return;
        if (BaseCam.position.x < 9 || BaseCam.position.x > -9)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                BaseCam.Translate(-touchDeltaPosition.x * 0.05f, 0, 0);
            }
        }

        if (BaseCam.position.x > 9) BaseCam.position = new Vector3(8.9f, BaseCam.position.y, -12.88f);
        if (BaseCam.position.x < -9) BaseCam.position = new Vector3(-8.9f, BaseCam.position.y, -0.31f);
    }
}
