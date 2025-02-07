using System;
using Godot;

namespace LittleDragon.scripts.map.grid;

/// <summary>
/// Represents a chunk in the grid-based map, storing position, type, and connection data.
/// </summary>
public class Chunk
{
    /// <summary>
    /// A 4-bit binary string representation of the chunk's connections (Left-Up-Right-Down).
    /// </summary>
    public string Binary { get; private set; }

    /// <summary>
    /// Internal integer representation of the chunk's connections.
    /// </summary>
    private int _binary;

    /// <summary>
    /// The name or identifier of the chunk.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The grid position of this chunk.
    /// </summary>
    public readonly Vector2I Position;

    /// <summary>
    /// Indicates whether this chunk is a special predefined chunk.
    /// </summary>
    internal readonly bool SpecialChunk;

    /// <summary>
    /// Initializes a standard chunk with a position and an I/O (connection) value.
    /// </summary>
    /// <param name="position">Grid position of the chunk.</param>
    /// <param name="ios">Initial I/O value representing connections.</param>
    internal Chunk(Vector2I position, int ios)
    {
        Position = position;
        Name = ios.ToString();
    }

    /// <summary>
    /// Initializes a special chunk with a custom name and I/O configuration.
    /// </summary>
    /// <param name="position">Grid position of the chunk.</param>
    /// <param name="ios">Initial I/O value representing connections.</param>
    /// <param name="name">Custom name for the special chunk.</param>
    internal Chunk(Vector2I position, int ios, string name)
    {
        SpecialChunk = true;
        Position = position;
        _binary = ios;
        Name = name;
    }

    /// <summary>
    /// Updates the chunk's I/O configuration and recalculates its binary representation.
    /// </summary>
    /// <param name="io">Bitmask representing the new connection direction.</param>
    internal void SetIo(int io)
    {
        _binary |= io;
        Name = !SpecialChunk ? (_binary & 0x0F).ToString() : Name;
        Binary = Convert.ToString(_binary, 2).PadLeft(4, '0');
    }
}