using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LittleDragon.scripts.map.grid;
using LittleDragonAlpha.scripts.map.grid;
using FileAccess = Godot.FileAccess;

namespace LittleDragon.scripts.map;

/// <summary>
/// Handles the loading and instantiation of room chunks in the grid.
/// </summary>
public static class ChunkLoader
{
    private const string RoomsPath = "res://assets/resources/rooms/";

    /// <summary>
    /// Instantiates the grid by creating and linking room instances based on the given grid structure.
    /// </summary>
    /// <param name="root">The root node to which instantiated rooms will be added.</param>
    /// <param name="grid">The grid structure containing chunk data.</param>
    /// <returns>A list of instantiated <see cref="Room"/> objects.</returns>
    public static List<Room> InstantiateGrid(Node2D root, Grid grid)
    {
        var rooms = new Room[grid.Chunks.GetLength(1), grid.Chunks.GetLength(0)];

        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var room = InstantiateRoom(root, grid.GetChunk(x, y));
                if (room == null) continue;
                rooms[x, y] = room;

                // Set neighbors in all four cardinal directions
                if (x > 0) room.SetNeighbor(0, rooms[x - 1, y]); // Left
                if (y < rooms.GetLength(1) - 1) room.SetNeighbor(1, rooms[x, y + 1]); // Down
                if (x < rooms.GetLength(0) - 1) room.SetNeighbor(2, rooms[x + 1, y]); // Right
                if (y > 0) room.SetNeighbor(3, rooms[x, y - 1]); // Up
            }
        }

        // Filter out null or empty rooms (rooms represented as "0000")
        return rooms.Cast<Room>().Where(room => room != null && room.ToString() != "0000").ToList();
    }

    /// <summary>
    /// Instantiates a room based on the provided chunk and attaches it to the given root node.
    /// </summary>
    /// <param name="root">The root <see cref="Node2D"/> where the room will be added.</param>
    /// <param name="chunk">The <see cref="Chunk"/> containing information about the room to be instantiated.</param>
    /// <returns>The instantiated <see cref="Room"/> if successful; otherwise, null.</returns>
    private static Room InstantiateRoom(Node2D root, Chunk chunk)
    {
        // If the chunk's name is "0", return null (indicating no room should be instantiated).
        if (chunk.Name == "0") return null;

        // Construct the path where the room scene files are stored.
        var path = chunk.SpecialChunk
            ? $"{RoomsPath}{chunk.Name}/{chunk.Binary}/"
            : $"{RoomsPath}{chunk.Name}_{chunk.Binary}/";

        // Attempt to open the directory containing room scenes.
        var dir = DirAccess.Open(path);
        if (dir == null) return null;

        // Count the number of available scene files in the directory.
        var count = 0;
        dir.ListDirBegin();
        var fileName = dir.GetNext();
        while (!string.IsNullOrEmpty(fileName))
        {
            if (!dir.CurrentIsDir()) count++;
            fileName = dir.GetNext();
        }

        // Select a random room file from the available options.
        var roomName = new Random().Next(0, count);
        var file = $"{path}{roomName}.tscn";

        // Ensure the selected room file exists.
        if (!FileAccess.FileExists(file)) return null;

        // Load and instantiate the scene.
        var scene = GD.Load<PackedScene>(file).Instantiate();

        // Attach the instantiated scene to the root node.
        root.AddChild(scene);

        // Return the Room node within the instantiated scene.
        return scene.GetNode<Room>("Room");
    }
}