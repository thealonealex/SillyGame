using UnityEngine;

public class EarthGravity : MonoBehaviour
{
    [SerializeField] private float gravityMagnitude = 10f;

    [SerializeField] private Vector3 speed = new Vector3(0, 1, 0); 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var speedAngle = Vector3.Angle(transform.right, transform.position);
        var gravityVector = Quaternion.AngleAxis(speedAngle - 90, Vector3.forward) * Vector3.right;
        speed = speed +  gravityVector * gravityMagnitude;
        speed.Normalize();
        transform.Translate(speed * gravityMagnitude * Time.deltaTime);
    }
}
