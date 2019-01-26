using UnityEngine;

namespace Geekbrains
{
	public class TestComponent : ITick
	{
		public TestComponent( )
		{
			
		}

		public void OnUpdate()
		{
			Debug.Log("Move");
		}
	}

	public interface ITick
	{
		void OnUpdate();
	}

	public class Main : MonoBehaviour
	{
		private ITick[] _ticks;

		private void Start()
		{
			var testComponent = new TestComponent();
			_ticks[0] = testComponent;
			_ticks = new ITick[1];
		}

		public void AddControl(object control)
		{
			
		}

		private void Update()
		{
			for (var i = 0; i <= _ticks.Length; i++)
			{
				_ticks[i].OnUpdate();
			}
		}
	}
}