﻿namespace FantasyFootball.ViewModels;

[QueryProperty(nameof(CompetitionId), nameof(CompetitionId))]
public partial class CompetitionDetailViewModel : GeneralViewModel
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Competition))]
	int _competitionId;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Stages))]
	[NotifyPropertyChangedFor(nameof(Winner))]
	Competition _competition = new();

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Rounds))]
	Stage? _selectedStage = new();

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(GamesByRound))]
	Round? _selectedRound = new();

	public IList<Stage> Stages => Competition.Stages;
	public IList<Round> Rounds => SelectedStage?.Rounds ?? [];

	public ObservableCollection<RoundGroup> GamesByRound { get; private set; }

	public Team? Winner => Competition.IsFinished ? Competition.LastGame?.Winner : null;

	public CompetitionSimulator Simulator { get; private set; }

	public string DisplayName => $"{Competition.ShortName} {Competition?.Id}";

	public CompetitionDetailViewModel()
	{
		// Will currently update ALL CompetitionViewModels regardless if it is the one that was finished
		MessageBus.Register<CompetitionFinishedMessage>(this, (_, _) => OnPropertyChanged(nameof(Competition)));
	}

	partial void OnCompetitionIdChanged(int value) => LoadCompetition();

	public virtual void LoadCompetition()
	{
		try
		{
			var loadedCompetitionFromDbById = Repo.Get<Competition>(CompetitionId);
			if (loadedCompetitionFromDbById is null)
			{
				Log.Error($"Unable to find {CompetitionId} in db");
				return;
			}
			Competition = loadedCompetitionFromDbById;
			GamesByRound = new(Competition.Rounds.Select(r => new RoundGroup(r.Name, r.AllGames.OrderBy(g => g.PlayedOn).Select(g => new GameViewModel(g)))));
			Simulator = new CompetitionSimulator(Competition, Repo);
			Title = $"{Competition.ShortName}-{Competition.Id}";
		}
		catch (Exception e)
		{
			Log.Error($"Failed to load competition: {e}");
		}
	}

	[RelayCommand]
	async Task DeleteCompetition()
	{
		Repo.Delete(Competition);
		await Shell.Current.GoToAsync(nameof(CompetitionsPage));
	}
}
