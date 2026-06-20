using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    public float dampTime = 0.15f;
	private Vector3 _velocity = Vector3.zero;
	public Transform target;

	private Camera _mainCamera;
	[SerializeField] private float verticalOffset = 0;

	// Update is called once per frame
	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void Update ()
	{
		if (!target) return;
		if (_mainCamera)
		{
			var point = _mainCamera.WorldToViewportPoint(target.position);
			var delta = target.position - _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			var destination = transform.position + delta + Vector3.up * verticalOffset;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, dampTime);
		}

		transform.Rotate(0,0,-transform.rotation.z);
	} 
}
