﻿using FantasyFootball.Data.CompetitionFactories;

namespace FantasyFootball.Services;

/// <summary>
/// Retrieve data from external sources - APIs, files (csv, txt ...)
/// </summary>
public interface IDataService
{
	/// <summary> Provides access to all available teams. Implementors should ensure that this is updated whenever a Team is changed (Name, Elo, etc.) </summary>
	List<Team> AllTeams { get; }

	CompetitionFactory CompetitionFactory { get; set; }

	List<Team> CreateTeams();

	/// <summary> Important: Only call once in app lifecycle! </summary>
	void Reset();

}
