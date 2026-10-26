using UnityEngine;

public class BeltController : MonoBehaviour
{
    public Transform[] pulleys; // Shkiv1, Shkiv2, Shkiv3
    public float rotationSpeed = 100f;
    public LineRenderer beltVisual;

    private bool isRotating = false;

    private void Start()
    {
        UpdateBeltPath();
    }

    private void Update()
    {
        if (isRotating)
        {
            foreach (var pulley in pulleys)
            {
                pulley.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void UpdateBeltPath()
    {
        if (beltVisual == null) return;

        beltVisual.positionCount = pulleys.Length;
        for (int i = 0; i < pulleys.Length; i++)
        {
            beltVisual.SetPosition(i, pulleys[i].position);
        }
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}