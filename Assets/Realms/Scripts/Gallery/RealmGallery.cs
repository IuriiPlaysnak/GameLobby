using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealmGallery : MonoBehaviour {
	
	[SerializeField]
	private RawImage _display;

	[SerializeField]
	private Text _title;

	[SerializeField]
	private Text _description;

	public event System.Action OnImageReady;

	private int _currentImage;

	void Awake() {

		Debug.Assert (_display != null, "ImageDisplay is missing");
		Debug.Assert (_title != null, "Title text field is missing");
		Debug.Assert (_description != null, "Description text field is missing");

		_display.enabled = false;
	}

	private List<OutrunRealmDataProvider.ImageData> _images;
	public void SetImages(List<OutrunRealmDataProvider.ImageData> images, bool doShowImmidiatly) {

		_currentImage = 0;
		_images = images;

		if (doShowImmidiatly)
			LoadImage (_images [_currentImage]);
	}

	private void LoadImage(OutrunRealmDataProvider.ImageData data) {

		_title.text = data.title;
		_description.text = data.description;

		_display.enabled = false;

		ResourceManager.LoadImage (data.url, OnImageLoaded);
	}
		
	public void Show() {
	
		LoadImage (_images [_currentImage]);
	}

	public void PrevImage() {

		_currentImage--;
		if (_currentImage < 0)
			_currentImage = _images.Count - 1;

		LoadImage (_images [_currentImage]);
	}

	public void NextImage() {

		if (_images == null || _images.Count == 0)
			return;

		_currentImage++;
		if (_currentImage > _images.Count - 1)
			_currentImage = 0;

		LoadImage (_images [_currentImage]);
	}

	void OnImageLoaded (Texture2D texture)
	{
		_display.texture = texture;
		_display.enabled = true;

		if (OnImageReady != null)
			OnImageReady ();
	}

	void OnDestroy() {

		OnImageReady = null;
	}
}