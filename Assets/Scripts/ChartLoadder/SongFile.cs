
using System;
using System.Collections.Generic;
using System.IO;

class SongFile : IChartGeneral, IChartMetadata, IChartObjects
{
    // General Header
    public string AudioFilename { get; private set; }
    public int PreviewTime { get; private set; }

    // Meta Data
    public string Title { get; private set; }
    public string Artist { get; private set; }
    public string Creator { get; private set; }
    public string Version { get; private set; }
    public string Source { get; private set; }

    // Note Pattern
    public ChartObject[] Objects { get; private set; }

    public bool Parse(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("파일이 존재하지 않습니다.");
            return false;
        }

        var lines = File.ReadAllLines(path);
        var chartObjects = new List<ChartObject>();

        string currentSection = string.Empty;

        foreach (var line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("//"))
                continue;

            if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
            {
                currentSection = trimmed.Trim('[', ']');
                continue;
            }

            switch (currentSection)
            {
                case "General":
                    ParseGeneral(trimmed);
                    break;

                case "Metadata":
                    ParseMetadata(trimmed);
                    break;

                case "HitObjects":
                    var obj = ParseHitObject(trimmed);
                    if (obj != null)
                        chartObjects.Add(obj.Value);
                    break;
            }
        }

        Objects = chartObjects.ToArray();
        return true;
    }

    private void ParseGeneral(string line)
    {
        if (line.Contains(":"))
        {
            var parts = line.Split(':');
            var key = parts[0].Trim();
            var value = parts[1].Trim();

            switch (key)
            {
                case "AudioFilename":
                    AudioFilename = value;
                    break;
                case "PreviewTime":
                    PreviewTime = int.TryParse(value, out var time) ? time : 0;
                    break;
            }
        }
    }

    private void ParseMetadata(string line)
    {
        if (line.Contains(":"))
        {
            var parts = line.Split(':');
            var key = parts[0].Trim();
            var value = parts[1].Trim();

            switch (key)
            {
                case "Title":
                    Title = value;
                    break;
                case "Artist":
                    Artist = value;
                    break;
                case "Creator":
                    Creator = value;
                    break;
                case "Version":
                    Version = value;
                    break;
                case "Source":
                    Source = value;
                    break;
            }
        }
    }

    private ChartObject? ParseHitObject(string line)
    {
        var parts = line.Split(',');

        if (parts.Length < 3)
            return null;

        if (!int.TryParse(parts[2], out var timing))
            return null;

        // NotePosition 매핑
        NotePosition position = NotePosition.None;
        switch (parts[0])
        {
            case "64":
                position = NotePosition.L1;
                break;
            case "192":
                position = NotePosition.L2;
                break;
            case "320":
                position = NotePosition.R1;
                break;
            case "448":
                position = NotePosition.R2;
                break;
        }

        var audioTexts = parts[5].Split(":");
        return new ChartObject
        {
            Timing = timing,
            Position = position,
            AudioPath = audioTexts.Length > 4 ? audioTexts[4] : string.Empty
        };
    }
}
