using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class YoutubeSnippet  {

    public string publishedAt;
    public string channelId;
    public string title;
    public string description;
    public YoutubeTumbnails thumbnails;
    public string channelTitle;
    public string[] tags;
    public string categoryId;
}

[System.Serializable]
public class YoutubeTumbnails
{
    public YoutubeThumbnailData defaultThumbnail;
    public YoutubeThumbnailData mediumThumbnail;
    public YoutubeThumbnailData highThumbnail;
    public YoutubeThumbnailData standardThumbnail;
}

[System.Serializable]
public class YoutubeThumbnailData
{
    public string url;
    public string width;
    public string height;
}

