using UnityEngine;

namespace Geekbrains
{
	public class PlayerProgress : MonoBehaviour
	{
		private int _level = 1; //уровень персонажа
		private int _statPoints; //текущее количество опыта
		private float _exp; //опыт, необходимый для взятия следующего уровня
		private float _nextLevelExp = 100; //количество свободных очков характеристик для механики прокачки персонажа

		public void AddExp(float addExp)
		{
			_exp += addExp;
			while (_exp >= _nextLevelExp)
			{
				_exp -= _nextLevelExp;
				LevelUp();
			}

			if (_manager != null)
			{
				_manager.Exp = _exp;
				_manager.NextLevelExp = _nextLevelExp;
				_manager.Level = _level;
				_manager.StatPoints = _statPoints;
			}
		}

		private void LevelUp()
		{
			_level++;
			_nextLevelExp += 100f;
			_statPoints += 3;
		}

		// менеджер характеристик
		private StatsManager _manager;

		// обновление синхронизируемых полей при установке менеджера
		public StatsManager Manager
		{
			set
			{
				_manager = value;
				_manager.Exp = _exp;
				_manager.NextLevelExp = _nextLevelExp;
				_manager.Level = _level;
				_manager.StatPoints = _statPoints;
			}
		}

		public bool RemoveStatPoint()
		{
			if (_statPoints > 0)
			{
				_statPoints--;
				if (_manager != null) _manager.StatPoints = _statPoints;
				return true;
			}
			return false;
		}
	}
}