using Godot;
using LittleDragon.scripts.map.grid;

namespace LittleDragonAlpha.scripts.map.grid;

/// <summary>
/// Represents a 2D grid composed of chunks, used for map generation.
/// </summary>
public class Grid
{
    /// <summary>
    /// The height of the grid in chunks.
    /// </summary>
    public readonly int Height;

    /// <summary>
    /// The width of the grid in chunks.
    /// </summary>
    public readonly int Width;

    /// <summary>
    /// The 2D array of chunks that make up the grid.
    /// </summary>
    public readonly Chunk[,] Chunks;

    /// <summary>
    /// Initializes a new grid with the specified dimensions, filling it with default chunks.
    /// </summary>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    internal Grid(int width, int height)
    {
        Chunks = new Chunk[height, width];
        Height = height;
        Width = width;

        // Populate the grid with default chunks
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                Chunks[y, x] = new Chunk(new Vector2I(y, x), 0);
            }
        }
    }

    /// <summary>
    /// Checks if a given position is out of the grid bounds.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns>True if the position is out of bounds, otherwise false.</returns>
    private bool OutOfBounds(Vector2I position) => 
        position.X >= Width || position.X < 0 || position.Y >= Height || position.Y < 0;

    /// <summary>
    /// Allocates a chunk into the grid at its specified position.
    /// </summary>
    /// <param name="chunk">The chunk to allocate.</param>
    internal void Alloc(Chunk chunk)
    {
        if (OutOfBounds(chunk.Position))
        {
            GD.PushError($"{chunk.Position} is out of bounds");
            return;
        }

        Chunks[chunk.Position.Y, chunk.Position.X] = chunk;
    }

    /// <summary>
    /// Retrieves a chunk at the specified grid position.
    /// </summary>
    /// <param name="position">The position of the chunk.</param>
    /// <returns>The chunk at the specified position.</returns>
    public Chunk GetChunk(Vector2I position) => Chunks[position.Y, position.X];

    /// <summary>
    /// Retrieves a chunk at the specified grid coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the chunk.</param>
    /// <param name="y">The y-coordinate of the chunk.</param>
    /// <returns>The chunk at the specified coordinates.</returns>
    public Chunk GetChunk(int x, int y) => Chunks[y, x];

    /// <summary>
    /// Returns a string representation of the grid, displaying chunk names and binary values.
    /// </summary>
    /// <returns>A formatted string representing the grid.</returns>
    public override string ToString()
    {
        var gridString = "";

        for (var y = 0; y < Height; y++)
        {
            gridString += "[";
            for (var x = 0; x < Width; x++)
            {
                var separate = GetChunk(x, y).SpecialChunk ? "_" : "";
                var end = x == Width - 1 ? "" : ", ";
                gridString += $"{GetChunk(x, y).Name}{separate}{GetChunk(x, y).Binary}{end}";
            }

            gridString += y == Height - 1 ? "]" : "],\n";
        }

        return gridString;
    }
}