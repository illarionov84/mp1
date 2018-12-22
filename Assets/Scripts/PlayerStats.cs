using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class PlayerStats : NetworkBehaviour
	{
		[SerializeField] private int _maxHealth;
		[SyncVar] private int _curHealth;

		public override void OnStartAuthority()
		{
			_curHealth = _maxHealth;
		}
	}
}