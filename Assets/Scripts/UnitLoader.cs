using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class UnitLoader : NetworkBehaviour
	{
		[SerializeField] private GameObject _unitPrefab;

		public override void OnStartServer()
		{
			var unit = Instantiate(_unitPrefab);
			NetworkServer.SpawnWithClientAuthority(unit, gameObject);
		}
	}
}