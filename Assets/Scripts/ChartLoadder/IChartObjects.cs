using UnityEditor;

public enum NotePosition
{
	L1, L2, 
	R1, R2,
	None,
};

public struct ChartObject
{
	public float Timing;
	public NotePosition Position;
	public string AudioPath; 
};

public interface IChartObjects
{
	public ChartObject[] Objects { get; }
}

