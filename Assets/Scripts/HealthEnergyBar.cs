// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthEnergyBar : MonoBehaviour {

	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private float maxEnergy = 150f;
	[SerializeField] private Color healthColor = Color.red;
	[SerializeField] private Color energyColor = Color.cyan;
	[SerializeField] private int healthWidth = 5;
	[SerializeField] private int energyWidth = 5;
	[SerializeField] private RectTransform healthRect;
	[SerializeField] private RectTransform energyRect;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Slider energySlider;
	[SerializeField] private Image healthFill;
	[SerializeField] private Image energyFill;
	[SerializeField] private bool healthRight;
	[SerializeField] private bool energyRight;

	private static float _currentHealth;
	private static float _currentEnergy;
	private static HealthEnergyBar _internal;

	public static float currentHealth
	{
		get{ return _currentHealth; }
	}

	public static float currentEnergy
	{
		get{ return _currentEnergy; }
	}

	public static HealthEnergyBar use
	{
		get{ return _internal; }
	}
	
	void Awake()
	{
		_internal = this;
		UpdateRectUI();
		if(healthFill) healthFill.color = healthColor;
		if(energyFill) energyFill.color = energyColor;
		if(healthSlider)
		{
			healthSlider.maxValue = maxHealth;
			healthSlider.minValue = 0;
			_currentHealth = maxHealth;
			AdjustCurrentHealth(0);
		}
		if(energySlider)
		{
			energySlider.maxValue = maxEnergy;
			energySlider.minValue = 0;
			_currentEnergy = maxEnergy;
			AdjustCurrentEnergy(0);
		}
	}

    public void UpdateRectUI()
	{
		if(healthRect)
		{
			int healthRectDeltaX = Screen.width/healthWidth;
			float healthRectPosX = 0;
			if(healthRight) healthRectPosX = healthRect.position.x - (healthRectDeltaX - healthRect.sizeDelta.x)/2;
			else healthRectPosX = healthRect.position.x + (healthRectDeltaX - healthRect.sizeDelta.x)/2;
			healthRect.sizeDelta = new Vector2(healthRectDeltaX, healthRect.sizeDelta.y);
			healthRect.position = new Vector3(healthRectPosX, healthRect.position.y, healthRect.position.z);
		}
		if(energyRect)
		{
			int energyRectDeltaX = Screen.width/energyWidth;
			float energyRectPosX = 0;
			if(energyRight) energyRectPosX = energyRect.position.x - (energyRectDeltaX - energyRect.sizeDelta.x)/2;
			else energyRectPosX = energyRect.position.x + (energyRectDeltaX - energyRect.sizeDelta.x)/2;
			energyRect.sizeDelta = new Vector2(energyRectDeltaX, energyRect.sizeDelta.y);
			energyRect.position = new Vector3(energyRectPosX, energyRect.position.y, energyRect.position.z);
		}
	}
	
	public void AdjustCurrentHealth(float adjust)
	{
		_currentHealth += adjust;
		if(_currentHealth < 0) _currentHealth = 0;
		if(_currentHealth > maxHealth) _currentHealth = maxHealth;
		healthSlider.value = _currentHealth;
	}
	
	public void AdjustCurrentEnergy(float adjust)
	{
		_currentEnergy += adjust;
		if(_currentEnergy < 0) _currentEnergy = 0;
		if(_currentEnergy > maxEnergy) _currentEnergy = maxEnergy;
		energySlider.value = _currentEnergy;
	}
}

