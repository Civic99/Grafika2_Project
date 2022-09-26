using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;

namespace Projekat2.Functionality
{
    public class BFS
    {
        Point startCoord;
        Point finishCoord;
        Point parentCoord;
        int row;
        int col;
        // All entities to search through
        static List<PowerEntity> entities;
        // Direction vectors
        static int[] dRow = { -1, 0, 1, 0 };
        static int[] dCol = { 0, 1, 0, -1 };
        public BFS()
        {
            startCoord = new Point(-1, -1);
            finishCoord = new Point(-1, -1);
            row = 0;
            col = 0;
        }

        public BFS(int first, int second, int pFirst, int pSecond)
        {
            startCoord = new Point(first, second);
            parentCoord = new Point(pFirst, pSecond);
        }

        public Point StartCoord { get => startCoord; set => startCoord = value; }
        public Point FinishCoord { get => finishCoord; set => finishCoord = value; }
        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public List<PowerEntity> Entities { get => entities; set => entities = value; }

        void getEntityStartCoordinates(long idStart)
        {
            foreach (PowerEntity item in Entities)
            {
                if (idStart == item.Id)
                    StartCoord = new Point(item.MatX, item.MatY);
            }
        }

        bool isValid(bool[,] vis, long[,] grid,
                            int row, int col)
        {

            // If cell lies out of bounds
            if (row < 0 || col < 0 ||
                row >= Row || col >= Col)
                return false;

            // If cell is already visited
            if (vis[row, col])
                return false;

            // If already line or entity
            if (grid[row, col] == 1)
                return false;

            // Otherwise
            return true;
        }


        public void BFSSearch(long idStart,long[,] grid, bool[,] vis, long idFinish)
        {
            BFS source = new BFS();
            source.getEntityStartCoordinates(idStart);
            if (startCoord.X == -1 || startCoord.Y == -1)
            {
                Console.WriteLine("StartNotFound");
                return;
            }
            // Stores indices of the matrix cells
            Queue<BFS> q = new Queue<BFS>();

            // Mark the starting cell as visited
            // and push it into the queue
            q.Enqueue(source);

            // Iterate while the queue
            // is not empty
            while (q.Count != 0)
            {
                BFS cell = q.Peek();
                int x = (int)startCoord.X;
                int y = (int)startCoord.Y;
                //podatci.Add(cell);
                if (grid[x, y] == idFinish)
                    break;

                q.Dequeue();

                // Go to the adjacent cells
                for (int i = 0; i < 4; i++)
                {
                    int adjx = x + dRow[i];
                    int adjy = y + dCol[i];
                    if (isValid(vis, grid, adjx, adjy))
                    {
                        q.Enqueue(new BFS(adjx, adjy, x, y));
                        vis[adjx, adjy] = true;
                    }
                }
            }
        }
    }
}
