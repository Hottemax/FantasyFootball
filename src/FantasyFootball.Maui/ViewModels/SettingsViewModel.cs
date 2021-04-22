﻿using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;

namespace FantasyFootball.ViewModels;

public partial class SettingsViewModel : GeneralViewModel
{
	public IList<CultureInfo> SupportedLanguages { get; init; }

	[ObservableProperty] CultureInfo _selectedLanguage;
	[ObservableProperty] double _simulationSpeedMs;

	readonly ISettingsService _settings;
	readonly IDataService _dataService;

	public SettingsViewModel(ISettingsService settingsService, IDataService dataService)
	{
		_settings = settingsService;
		_dataService = dataService;
		_selectedLanguage = settingsService.LastUsedLanguage;

		SimulationSpeedMs = _settings.SimulationSpeed.TotalMilliseconds;
		SupportedLanguages = new List<CultureInfo>() { new("en"), new("de"), };
	}

	public string AppVersion => AppInfo.VersionString;

	partial void OnSelectedLanguageChanged(CultureInfo value)
	{
		_settings.AddOrUpdateValue("Language", value.Name);
		LocalizationResourceManager.Current.CurrentCulture = value;
		CultureInfo.CurrentUICulture = value;
		CultureInfo.DefaultThreadCurrentCulture = value;
		CultureInfo.DefaultThreadCurrentUICulture = value;
	}

	partial void OnSimulationSpeedMsChanged(double value) => _settings.SimulationSpeed = TimeSpan.FromMilliseconds(value);

	[ICommand]
	void ResetDatabase() => _dataService.Reset();
}
