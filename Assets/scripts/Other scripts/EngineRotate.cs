using UnityEngine;

public class EngineRotate : MonoBehaviour
{
    public Transform[] rotatingParts;
    public float speed = 200f;

    private bool rotate = false;
    private AudioSource engineAudio;

    void Start()
    {
        engineAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!rotate) return;

        foreach (Transform part in rotatingParts)
        {
            part.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void StartRotation()
    {
        rotate = true;

        if (engineAudio != null)
            engineAudio.Play();
    }
}