using System;
using System.Collections.Generic;
using Godot;
using LittleDragon.scripts.map.grid;
using FileAccess = Godot.FileAccess;

namespace LittleDragon.scripts.map;

/// <summary>
/// Handles the loading and instantiation of room chunks in the grid.
/// </summary>
public static class ChunkManager
{
    private const string RoomsPath = "res://assets/resources/rooms/";

    /// <summary>
    /// Instantiates the grid by creating and linking room instances based on the given grid structure.
    /// </summary>
    /// <param name="root">The root node to which instantiated rooms will be added.</param>
    /// <param name="grid">The grid structure containing chunk data.</param>
    /// <returns>A list of instantiated <see cref="Chunk"/> objects.</returns>
    public static List<Chunk> InstantiateGrid(Node2D root, Grid grid)
    {
        var chunks = new Chunk[grid.Width, grid.Height];

        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                var room = InstantiateRoom(root, grid.GetChunk(x, y));
                if (room != null) chunks[x, y] = room;
            }
        }
        
        for (var y = 0; y < grid.Height; y++) 
        {
            for (var x = 0; x < grid.Width; x++) 
            {
                var chunk = chunks[x, y];
                
                // This code is really stinky, full, bizarre, horrible and poorly made.
                // But I'm tired and nothing I try to do to change it works, so it will stay like this.
                try { chunk.SetNeighbor(0, chunks[x - 1, y]); } catch { /* ignored */ } // Left
                try { chunk.SetNeighbor(1, chunks[x, y + 1]); } catch { /* ignored */ } // Up
                try { chunk.SetNeighbor(2, chunks[x + 1, y]); } catch { /* ignored */ } // Right
                try { chunk.SetNeighbor(3, chunks[x, y - 1]); } catch { /* ignored */ } // Down
            }
        }

        var list = new List<Chunk>();
        foreach (var room in chunks)
        {
            if(room == null) continue;
            room.GetOwner<Node2D>().Visible = false;
            list.Add(room);
        }
        return list;
    }

    /// <summary>
    /// Instantiates a room based on the provided chunk and attaches it to the given root node.
    /// </summary>
    /// <param name="root">The root <see cref="Node2D"/> where the room will be added.</param>
    /// <param name="room">The <see cref="Room"/> containing information about the room to be instantiated.</param>
    /// <returns>The instantiated <see cref="Chunk"/> if successful; otherwise, null.</returns>
    private static Chunk InstantiateRoom(Node2D root, Room room)
    {
        // TODO: select a random scene
        const int fileId = 0;

        var file = $"{RoomsPath}{room.Name}_{room.Binary}/{fileId}.tscn";
        if (!FileAccess.FileExists(file)) return null;
        
        var scene = GD.Load<PackedScene>(file).Instantiate();
        root.AddChild(scene);
        
        return scene.GetNode<Chunk>("Chunk");
    }

    /// <summary>
    /// Instantiate Room unitary test
    /// </summary>
    /// <param name="node2D">a root to instance the rooms and use to test</param>
    public static void Test_InstantiateRoom(Node2D node2D)
    {
        var roomInts = new[]
        {
            0b0011, 0b0101, 0b0110, 0b0111, 0b1001, 0b1010, 0b1011, 0b1100, 0b1101, 0b1110, 0b1111
        };

        foreach (var t in roomInts)
        {
            var room = new Room(Vector2I.Zero, t);
            var instantiateRoom = InstantiateRoom(node2D, room);
            var bin = Convert.ToString(t, 2).PadLeft(4, '0');
            var bin2 = Convert.ToString(instantiateRoom.ToInt(), 2).PadLeft(4, '0');
            var isCorrect = bin2 == bin; 
            GD.Print($"{isCorrect} => {bin2} ==: {room.Name}_{room.Binary}");
            instantiateRoom.GetOwner<Node2D>().QueueFree();
        }
    }
}