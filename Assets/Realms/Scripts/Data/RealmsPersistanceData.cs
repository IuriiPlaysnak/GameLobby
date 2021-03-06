﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PlaysnakRealms {

	public static class RealmsPersistenceData  {

		private const string FILE_NAME = "playerData.dat";

		public static List<PlayerData> savedGames = new List<PlayerData>();

		private static string _filePath = string.Empty;

		public static void SaveChairHeight(float height) {
			PlayerData data = LoadPlayerData ();
			data.height = height;
			SavePlayerData (data);
		}

		public static float LoadChairHeight() {
			PlayerData data = LoadPlayerData ();
			return data.height;
		}

		public static void SaveMusicVolume(int volume) {
			PlayerData data = LoadPlayerData ();
			data.musicVolume = volume;
			SavePlayerData (data);
		}

		public static int LoadMusicVolume() {
			PlayerData data = LoadPlayerData ();
			return data.musicVolume;
		}

		public static void SaveSfxVolume(int volume) {
			PlayerData data = LoadPlayerData ();
			data.sfxVolume = volume;
			SavePlayerData (data);
		}

		public static int LoadSfxVolume() {
			PlayerData data = LoadPlayerData ();
			return data.sfxVolume;
		}

		public static bool doShowNewPlayerContent {

			get {

				PlayerData data = LoadPlayerData ();
				return data.doShowNewPlayerContent;
			}

			set {

				PlayerData data = LoadPlayerData ();
				data.doShowNewPlayerContent = value;
				SavePlayerData (data);
			}
		}

		private static bool _doUseCachedData;
		private static PlayerData _cachedData;

		private static void SavePlayerData(PlayerData data) {

			if (_filePath == string.Empty)
				_filePath = string.Format ("{0}/{1}", Application.persistentDataPath, FILE_NAME);

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (_filePath);
			bf.Serialize(file, data);
			file.Close();

			_cachedData = data;
			_doUseCachedData = true;
		}   

		private static PlayerData LoadPlayerData() {

			if (_doUseCachedData == false) {
				
				_doUseCachedData = true;

				if (_filePath == string.Empty)
					_filePath = string.Format ("{0}/{1}", Application.persistentDataPath, FILE_NAME);

				if (File.Exists (_filePath)) {
				
					BinaryFormatter bf = new BinaryFormatter ();
					FileStream file = File.Open (_filePath, FileMode.Open);
					_cachedData = (PlayerData)bf.Deserialize (file);
					file.Close ();

				} else {

					_cachedData = new PlayerData () 
					{ 
						height = 0.65f
							, musicVolume = 100
							, sfxVolume = 100
							, doShowNewPlayerContent = true 
					};
				}
			}

			return _cachedData;
		}

		[System.Serializable]
		public struct PlayerData {
			public float height;
			public int musicVolume;
			public int sfxVolume;
			public bool doShowNewPlayerContent;

			public override string ToString ()
			{
				return string.Format (
					"[PlayerData]: height = {0}, music = {1}, sfx = {2}, show new player content = {3}"
					, height
					, musicVolume
					, sfxVolume
					, doShowNewPlayerContent
				);
			}
		}
	}
}