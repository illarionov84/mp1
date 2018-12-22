using UnityEngine;

namespace Geekbrains
{
	[RequireComponent(typeof(UnitMotor))]
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private LayerMask _movementMask;

		private Camera _cam;
		private UnitMotor _motor;

		private void Start()
		{
			_cam = Camera.main;
			_motor = GetComponent<UnitMotor>();
			_cam.GetComponent<CameraController>().Target = transform;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				var ray = _cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out var hit, 100f, _movementMask))
				{
					_motor.MoveToPoint(hit.point);
				}
			}

			if (!Input.GetMouseButtonDown(0)) return;
			{
				var ray = _cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out var hit, 100f))
				{

				}
			}
		}
	}
}
