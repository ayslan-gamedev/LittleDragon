using System;
using System.Collections.Generic;
using System.Linq;
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
        var rooms = new Chunk[grid.Width, grid.Height];

        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                var room = InstantiateRoom(root, grid.GetChunk(x, y));
                GD.Print($"{room}: {x}, {y}");
                if (room != null) rooms[x, y] = room;
            }
        }

        GD.Print("finished");
        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var room = rooms[x, y];
                GD.Print(x + " " + y);
            }
        }

        var list = new List<Chunk>();
        foreach (var room in rooms)
        {
            if(room == null) continue;
            //room.GetOwner<Node2D>().Visible = false;
            GD.Print(room);
            list.Add(room);
        }
        GD.Print("ok");
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
        // If the chunk's name is "0", return null (indicating no room should be instantiated).
        if (room.Name == "0") return null;

        // Construct the path where the room scene files are stored.
        var path = room.SpecialRoom
            ? $"{RoomsPath}{room.Name}/{room.Binary}/"
            : $"{RoomsPath}{room.Name}_{room.Binary}/";

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
        return scene.GetNode<Chunk>("Room");
    }
    
    public static void SetCurrentChunk(Chunk chunk, List<Chunk> rooms)
    {
        var currentRoom = new Chunk();

        foreach (var mRoom in rooms)
        {
            if (mRoom != currentRoom)
            {
                mRoom.GetOwner<Node2D>().Visible = false;
            }
            else
            {
                currentRoom = mRoom;
                currentRoom.GetOwner<Node2D>().Visible = true;
            }
        }
        
        foreach (var neighbor in currentRoom.NeighborRooms)
        {
            if (neighbor != null) 
                neighbor.GetOwner<Node2D>().Visible = true;
        }            
    }
}