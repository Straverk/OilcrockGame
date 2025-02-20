using UnityEngine;
using GameSettings.Configs;
using SaveUtils;
using System;
using GameSettings.Configs.Annum;

public class DaySystem : MonoBehaviour
{
    [Serializable, Flags]
    private enum Luminary—ontext
    {
        Sun = 1,
        Moon = 1 << 2
    }

    [Header("Light sources")]
    [SerializeField] private Skybox _skyboxComponent;

    [SerializeField] private Light _sunComponent;
    [SerializeField] private Light _moonComponent;

    [Header("Light Settings")]
    [SerializeField, Tooltip("The angle at which rise begins")] private float _riseAngle = 210;
    [SerializeField, Tooltip("The angle at which set end")] private float _setAngle = -28;



    SaveLoadersManager _saveLoadersManager;
    private AnnumsConfig _annumsConfig;



    private SeasonConfig _season;
    private DateTime _dateTime;
    private int _lastMonth = 0;
    private int _lastMinute = 0;
    private float _dayHoursLength;
    private float _nightHoursLength;

    [SerializeField] private Luminary—ontext _luminary—ontext;



    public void Init(SaveLoadersManager saveLoadersManager, AnnumsConfig annumsConfig)
    {
        _saveLoadersManager = saveLoadersManager;
        _annumsConfig = annumsConfig;

        /*
        var dt = annumsConfig.DefaultDate;
        _saveLoadersManager.SeasonSaveLoader.DateTime =
            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);

        Debug.Log("DaySystem Init!");
        Debug.Log(_saveLoadersManager.SeasonSaveLoader.DateTime == null);
        Debug.Log(_saveLoadersManager.SeasonSaveLoader.DateTime.Year);
        Debug.Log(_saveLoadersManager.SeasonSaveLoader.DateTime.Month);
        Debug.Log(_saveLoadersManager.SeasonSaveLoader.DateTime.Day);
        */

        if (_saveLoadersManager.SeasonSaveLoader.DateTime
            == new DateTime(2025, 1, 1))
        {
            Debug.LogAssertion("Init Date");

            var dt = annumsConfig.DefaultDate;
            _saveLoadersManager.SeasonSaveLoader.DateTime =
                new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }

        _dateTime = _saveLoadersManager.SeasonSaveLoader.DateTime;

        UpdateDateTime(0);
    }



    private void FixedUpdate()
    {
        UpdateDateTime(Time.fixedDeltaTime);
        UpdateLuminaryPlace();
    }


    private void UpdateDateTime(float seconds)
    {
        _dateTime = _dateTime.AddSeconds(seconds * _annumsConfig.DayScaler);

        if (_dateTime.Minute != _lastMinute)
        {
            UpdateLuminary—ontext();
            _lastMinute = _dateTime.Minute;
        }
        if (_dateTime.Month != _lastMonth)
        {
            UpdateLuminarySettings();
            _lastMonth = _dateTime.Month;
        }
    }

    private void UpdateLuminarySettings()
    {
        _season = _annumsConfig.GetSeasonConfig(_dateTime);

        _skyboxComponent.material = _season.SkyboxShaderMaterial;

        _dayHoursLength = _season.DayEndTime - _season.DayStartTime;

        _nightHoursLength = _season.NightEndTime + 24 - _season.NightStartTime;
    }

    private void UpdateLuminary—ontext()
    {
        _luminary—ontext = 0;

        var time = _dateTime.Hour + (_dateTime.Minute / 60f);

        if (time > _season.DayStartTime && _dateTime.Hour < _season.DayEndTime)
        {
            _luminary—ontext |= Luminary—ontext.Sun;
            _sunComponent.gameObject.SetActive(true);
        }
        else
            _sunComponent.gameObject.SetActive(false);


        if (time > _season.NightStartTime || _dateTime.Hour < _season.NightEndTime)
        {
            _luminary—ontext |= Luminary—ontext.Moon;
            _moonComponent.gameObject.SetActive(true);
        }
        else
            _moonComponent.gameObject.SetActive(false);
    }

    private void UpdateLuminaryPlace()
    {
        var time = _dateTime.Hour + (_dateTime.Minute / 60f) +
            (_dateTime.Second / 3200f) + (_dateTime.Millisecond / 3200000f);

        if ((_luminary—ontext & Luminary—ontext.Sun) != 0)
        {
            var sunPassed = (time - _season.DayStartTime) / _dayHoursLength;

            _sunComponent.transform.rotation = Quaternion.Euler(Vector3.right *
                    Mathf.Lerp(_riseAngle, _setAngle, sunPassed));

            _season.SetDayFactor(sunPassed);

            _sunComponent.colorTemperature = _season.SunTemperature *
                _season.sunTemperatureMultiplier.Evaluate(sunPassed);

            _sunComponent.intensity = _season.SunIntensity *
                _season.sunIntensityMultiplier.Evaluate(sunPassed);
        }

        if ((_luminary—ontext & Luminary—ontext.Moon) != 0)
        {
            var moonPassed = ((time > _season.NightEndTime ? time : time + 24) -
                _season.NightStartTime) / _nightHoursLength;

            _moonComponent.transform.rotation = Quaternion.Euler(Vector3.right *
                    Mathf.Lerp(_riseAngle, _setAngle, moonPassed));

            _moonComponent.colorTemperature = _season.MoonTemperature *
                _season.moonTemperatureMultiplier.Evaluate(moonPassed);

            _moonComponent.intensity = _season.MoonIntensity *
                _season.moonIntensityMultiplier.Evaluate(moonPassed);
        }
    }


}
