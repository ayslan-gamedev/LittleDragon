using System;
using System.Collections.Generic;
using Godot;

namespace LittleDragon.scripts.map.grid;

/// <summary>
/// Provides utility methods for creating grids of chunks.
/// </summary>
public abstract class GridFactory
{
    /// <summary>
    /// Creates a grid with random chunk placement based on the provided chunk builders and seed.
    /// </summary>
    /// <param name="width">The width of the grid in chunks.</param>
    /// <param name="height">The height of the grid in chunks.</param>
    /// <param name="dots">An array of <see cref="RoomBuilder"/> to define chunk placement.</param>
    /// <param name="seed">The seed for random number generation. Defaults to 0 for random seeding.</param>
    /// <returns>A tuple containing the created <see cref="Grid"/> and a list of <see cref="Vector2I"/> positions of the placed chunks.</returns>
    public static (Grid grid, List<Vector2I> dots) CreateGrid(int width, int height, RoomBuilder[] dots, int seed = 0)
    {
        var newGrid = new Grid(width, height);
        var rng = new Random(seed == 0 ? new Random().Next(0, 10000) : seed);

        var newDots = new List<Vector2I>();
        foreach (var builder in dots)
        {
            var position = Vector2I.Zero;
            do
            {
                position.X = rng.Next(builder.Min.X, builder.Max.X);
                position.Y = rng.Next(builder.Min.Y, builder.Max.Y);
            } while (newGrid.GetChunk(position).SpecialRoom);
            
            newGrid.Alloc(new Room(position, 0, builder.Name));
            newDots.Add(position);
        }
        
        return (newGrid, newDots);
    }

    /// <summary>
    /// Connects two points in the grid with a path of chunks.
    /// </summary>
    /// <param name="grid">The grid in which to create the path.</param>
    /// <param name="start">The starting position of the path.</param>
    /// <param name="end">The ending position of the path.</param>
    public static void Connect(Grid grid, Vector2I start, Vector2I end)
    {
        var lastDirection = -100;
        while (true)
        {
            if (lastDirection != -100) grid.GetChunk(start).SetIo(lastDirection);

            var distance = end - start;
            var lookup = new[,]
            {
                { -1, 1, 0, 0 }, // direction X
                { 0, 0, 1, -1 }, // direction Y
                { 0b1000, 0b0010, 0b0001, 0b0100 }, // next direction
                { 0b0010, 0b1000, 0b0100, 0b0001 }, // last direction
            };
            
            // TODO Add a way to divide path

            // x < 0 = 1, x > 0 = 0, y > o 2, y < 0 = 3
            var index = distance.X != 0 ? distance.X < 0 ? 0 : 1  : distance.Y < 0 ? 3 : 2;

            if (distance.X != 0 || distance.Y != 0)
            {
                grid.GetChunk(start).SetIo(lookup[2, index]);
                lastDirection = lookup[3, index];
                start += new Vector2I(lookup[0, index], lookup[1, index]);
                continue;
            }
            break;
        }
    }
}