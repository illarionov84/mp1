using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
	public class StatsUI : MonoBehaviour
	{
		#region Singleton
		public static StatsUI instance;

		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("More than one instance of StatsUI found!");
				return;
			}
			instance = this;
		}
		#endregion

		[SerializeField] private GameObject _statsUi;
		[SerializeField] private StatItem _damageStat;
		[SerializeField] private StatItem _armorStat;
		[SerializeField] private StatItem _moveSpeedStat;
		// поля для вывода уровня и очков характеристик
		[SerializeField] private Text _levelText;
		[SerializeField] private Text _statPointsText;

		// установка возможности апгрейда для всех статов
		private void SetUpgradableStats(bool active)
		{
			_damageStat.SetUpgradable(active);
			_armorStat.SetUpgradable(active);
			_moveSpeedStat.SetUpgradable(active);
		}

		private StatsManager _manager;
		private int _curDamage, _curArmor, _curMoveSpeed;
		int _curLevel, _curStatPoints;
		float _curExp, _nextLevelExp;

		void Start()
		{
			_statsUi.SetActive(false);
		}

		void Update()
		{
			if (Input.GetButtonDown("Stats"))
			{
				_statsUi.SetActive(!_statsUi.activeSelf);
			}
			if (_manager != null)
			{
				CheckManagerChanges();
			}
		}

		public void SetManager(StatsManager statsManager)
		{
			_manager = statsManager;
			CheckManagerChanges();
		}

		private void CheckManagerChanges()
		{
			// stat changes
			if (_curDamage != _manager.Damage)
			{
				_curDamage = _manager.Damage;
				_damageStat.ChangeStat(_curDamage);
			}
			if (_curArmor != _manager.Armor)
			{
				_curArmor = _manager.Armor;
				_armorStat.ChangeStat(_curArmor);
			}
			if (_curMoveSpeed != _manager.MoveSpeed)
			{
				_curMoveSpeed = _manager.MoveSpeed;
				_moveSpeedStat.ChangeStat(_curMoveSpeed);
			}

			if (_curLevel != _manager.Level)
			{
				_curLevel = _manager.Level;
				_levelText.text = _curLevel.ToString();
			}
			if (_curExp != _manager.Exp)
			{
				_curExp = _manager.Exp;
			}
			if (_nextLevelExp != _manager.NextLevelExp)
			{
				_nextLevelExp = _manager.NextLevelExp;
			}
			if (_curStatPoints != _manager.StatPoints)
			{
				_curStatPoints = _manager.StatPoints;
				_statPointsText.text = _curStatPoints.ToString();
				SetUpgradableStats(_curStatPoints > 0);
			}
		}

		public void UpgradeStat(StatItem stat)
		{
			if (stat == _damageStat) _manager.CmdUpgradeStat((int)StatType.Damage);
			else if (stat == _armorStat) _manager.CmdUpgradeStat((int)StatType.Armor);
			else if (stat == _moveSpeedStat) _manager.CmdUpgradeStat((int)StatType.MoveSpeed);
		}
	}
}