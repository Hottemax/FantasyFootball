﻿namespace FantasyFootball.Repositories;

public interface IRepository : IDisposable

{
	IList<T> GetAll<T>() where T : NamedUniqueId, new();
	T Get<T>(int id) where T : NamedUniqueId, new();
	int Count<T>() where T : NamedUniqueId, new();
	void Save<T>(T item) where T : NamedUniqueId, new();
	void Reset();
}
