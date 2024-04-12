 #pragma warning disable CS1587 // XML comment is not placed on a valid language element
/**
    MIT License

    Copyright (c) 2018 Michael Vivet

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 *
 */
#pragma warning restore CS1587 // XML comment is not placed on a valid language element

using System;

namespace LethalAchievements.Config;

/// <summary>
/// Hungarian Algorithm.
/// </summary>
public static class HungarianAlgorithm
{
    /// <summary>
    /// Finds the optimal assignments for a given matrix of agents and costed tasks such that the total cost is minimized.
    /// </summary>
    /// <param name="costs">A cost matrix; the element at row <em>i</em> and column <em>j</em> represents the cost of agent <em>i</em> performing task <em>j</em>.</param>
    /// <returns>A matrix of assignments; the value of element <em>i</em> is the column of the task assigned to agent <em>i</em>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="costs"/> is null.</exception>
    public static int[] FindAssignments(this int[,] costs)
    {
        if (costs == null)
            throw new ArgumentNullException(nameof(costs));

        var h = costs.GetLength(0);
        var w = costs.GetLength(1);

        for (var i = 0; i < h; i++)
        {
            var min = int.MaxValue;

            for (var j = 0; j < w; j++)
            {
                min = Math.Min(min, costs[i, j]);
            }

            for (var j = 0; j < w; j++)
            {
                costs[i, j] -= min;
            }
        }

        var masks = new byte[h, w];
        var rowsCovered = new bool[h];
        var colsCovered = new bool[w];

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (costs[i, j] == 0 && !rowsCovered[i] && !colsCovered[j])
                {
                    masks[i, j] = 1;
                    rowsCovered[i] = true;
                    colsCovered[j] = true;
                }
            }
        }

        ClearCovers(rowsCovered, colsCovered, w, h);

        var path = new Location[w * h];
        var pathStart = default(Location);
        var step = 1;

        while (step != -1)
        {
            step = step switch
            {
                1 => RunStep1(masks, colsCovered, w, h),
                2 => RunStep2(costs, masks, rowsCovered, colsCovered, w, h, ref pathStart),
                3 => RunStep3(masks, rowsCovered, colsCovered, w, h, path, pathStart),
                4 => RunStep4(costs, rowsCovered, colsCovered, w, h),
                _ => step
            };
        }

        var agentsTasks = new int[h];

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (masks[i, j] == 1)
                {
                    agentsTasks[i] = j;
                    break;
                }
            }
        }

        return agentsTasks;
    }

    private static int RunStep1(byte[,] masks, bool[] colsCovered, int w, int h)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (masks[i, j] == 1)
                    colsCovered[j] = true;
            }
        }

        var colsCoveredCount = 0;

        for (var j = 0; j < w; j++)
        {
            if (colsCovered[j])
                colsCoveredCount++;
        }

        if (colsCoveredCount == h)
            return -1;

        return 2;
    }
    private static int RunStep2(int[,] costs, byte[,] masks, bool[] rowsCovered, bool[] colsCovered, int w, int h, ref Location pathStart)
    {
        if (costs == null)
            throw new ArgumentNullException(nameof(costs));

        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        if (rowsCovered == null)
            throw new ArgumentNullException(nameof(rowsCovered));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        while (true)
        {
            var loc = FindZero(costs, rowsCovered, colsCovered, w, h);
            if (loc.row == -1)
                return 4;

            masks[loc.row, loc.column] = 2;

            var starCol = FindStarInRow(masks, w, loc.row);
            if (starCol != -1)
            {
                rowsCovered[loc.row] = true;
                colsCovered[starCol] = false;
            }
            else
            {
                pathStart = loc;
                return 3;
            }
        }
    }
    private static int RunStep3(byte[,] masks, bool[] rowsCovered, bool[] colsCovered, int w, int h, Location[] path, Location pathStart)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        if (rowsCovered == null)
            throw new ArgumentNullException(nameof(rowsCovered));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        var pathIndex = 0;
        path[0] = pathStart;

        while (true)
        {
            var row = FindStarInColumn(masks, h, path[pathIndex].column);
            if (row == -1)
                break;

            pathIndex++;
            path[pathIndex] = new Location(row, path[pathIndex - 1].column);

            var col = FindPrimeInRow(masks, w, path[pathIndex].row);

            pathIndex++;
            path[pathIndex] = new Location(path[pathIndex - 1].row, col);
        }

        ConvertPath(masks, path, pathIndex + 1);
        ClearCovers(rowsCovered, colsCovered, w, h);
        ClearPrimes(masks, w, h);

        return 1;
    }
    private static int RunStep4(int[,] costs, bool[] rowsCovered, bool[] colsCovered, int w, int h)
    {
        if (costs == null)
            throw new ArgumentNullException(nameof(costs));

        if (rowsCovered == null)
            throw new ArgumentNullException(nameof(rowsCovered));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        var minValue = FindMinimum(costs, rowsCovered, colsCovered, w, h);

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (rowsCovered[i])
                    costs[i, j] += minValue;
                if (!colsCovered[j])
                    costs[i, j] -= minValue;
            }
        }
        return 2;
    }

    private static int FindMinimum(int[,] costs, bool[] rowsCovered, bool[] colsCovered, int w, int h)
    {
        if (costs == null)
            throw new ArgumentNullException(nameof(costs));

        if (rowsCovered == null)
            throw new ArgumentNullException(nameof(rowsCovered));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        var minValue = int.MaxValue;

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (!rowsCovered[i] && !colsCovered[j])
                    minValue = Math.Min(minValue, costs[i, j]);
            }
        }

        return minValue;
    }
    private static int FindStarInRow(byte[,] masks, int w, int row)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        for (var j = 0; j < w; j++)
        {
            if (masks[row, j] == 1)
                return j;
        }

        return -1;
    }
    private static int FindStarInColumn(byte[,] masks, int h, int col)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        for (var i = 0; i < h; i++)
        {
            if (masks[i, col] == 1)
                return i;
        }

        return -1;
    }
    private static int FindPrimeInRow(byte[,] masks, int w, int row)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        for (var j = 0; j < w; j++)
        {
            if (masks[row, j] == 2)
                return j;
        }

        return -1;
    }
    private static Location FindZero(int[,] costs, bool[] rowsCovered, bool[] colsCovered, int w, int h)
    {
        if (costs == null)
            throw new ArgumentNullException(nameof(costs));

        if (rowsCovered == null)
            throw new ArgumentNullException(nameof(rowsCovered));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (costs[i, j] == 0 && !rowsCovered[i] && !colsCovered[j])
                    return new Location(i, j);
            }
        }

        return new Location(-1, -1);
    }
    private static void ConvertPath(byte[,] masks, Location[] path, int pathLength)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        if (path == null)
            throw new ArgumentNullException(nameof(path));

        for (var i = 0; i < pathLength; i++)
        {
            masks[path[i].row, path[i].column] = masks[path[i].row, path[i].column] switch
            {
                1 => 0,
                2 => 1,
                _ => masks[path[i].row, path[i].column]
            };
        }
    }
    private static void ClearPrimes(byte[,] masks, int w, int h)
    {
        if (masks == null)
            throw new ArgumentNullException(nameof(masks));

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (masks[i, j] == 2)
                    masks[i, j] = 0;
            }
        }
    }
    private static void ClearCovers(bool[] rowsCovered, bool[] colsCovered, int w, int h)
    {
        if (rowsCovered == null)
            throw new ArgumentNullException(nameof(rowsCovered));

        if (colsCovered == null)
            throw new ArgumentNullException(nameof(colsCovered));

        for (var i = 0; i < h; i++)
        {
            rowsCovered[i] = false;
        }

        for (var j = 0; j < w; j++)
        {
            colsCovered[j] = false;
        }
    }

    private struct Location
    {
        internal readonly int row;
        internal readonly int column;

        internal Location(int row, int col)
        {
            this.row = row;
            column = col;
        }
    }
}