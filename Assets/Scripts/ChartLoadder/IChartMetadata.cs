using UnityEngine;

public interface IChartMetadata
{
    public string Title { get; }
    public string Artist { get; }
    public string Creator { get; }
    public string Version { get; }
    public string Source { get; }
}