using System.Collections;
using System.Collections.Generic;
using System;

public interface IOutrunRealmDataSource {

	void Load (string url);
	event Action<OutrunRealmDataProvider.SettingData> OnLoadingComplete;
}