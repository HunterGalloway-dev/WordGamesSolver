using System;
using System.Collections;
using System.Collections.Generic;

namespace WorldPuzzle
{
    public class WorldPuzzleSolver
    {

        public struct Node
        {
            public Node(char val)
            {
                this.Val = val;
                this.Visited = false;
            }

            public char Val { get; set; }
            public bool Visited { get; set; }
        }

        private Node[,] CharGraph;
        private int Width;
        private int Height;
        private Dictionary<String, int> Words;
        private List<int[]> NeighborVectors = new List<int[]>() {
            new int[] {-1, 0}, // Top Middle
            new int[] {1, 0}, // Bottom Middle
            new int[] {0, -1}, // Left Middle
            new int[] {0, 1}, // Right Middle
            new int[] {-1,-1}, // Top Left
            new int[] {-1, 1}, // Top Right
            new int[] {1, -1}, // Bottom Left
            new int[] {1, 1}, // Bottom Right
        };

        public WorldPuzzleSolver(string input, int width, int height) {
            this.Width = width;
            this.Height = height;

            this.CharGraph = new Node[Height,Width];
            this.Words = new Dictionary<string, int>();
            char[] charArray = input.ToCharArray();

             int[] pos;
             for(int i = 0; i < charArray.Length; i++) {
                 pos = GetPosFromIndex(i);

                 CharGraph[pos[0],pos[1]] = new Node(charArray[i]);
             }

            System.IO.StreamReader file = new System.IO.StreamReader("dictionary.txt");  
            String line;
            while((line = file.ReadLine()) != null)  
            {  
                this.Words.Add(line,0); 
            }  
        }

        public List<String> GenWords() {
            List<String> ret = new List<String>();

            for(int r = 0; r < this.Height; r++) {
                for(int c = 0; c < this.Width; c++) {
                    PossWords(new int[]{r,c}, "", 7, ret, new int[] {r,c});
                }
            }

            return ret;
        }

        public void PossWords(int[] curPos, String curWord, int depth, List<String> possWords, int[] startPos) {
            if(ValidPos(curPos[0],curPos[1])) {
                if(curWord.Length > 3 && this.Words.ContainsKey(curWord) && !possWords.Contains($"[{startPos[0]},{startPos[1]}]:{curWord}")) {
                    possWords.Add($"[{startPos[0]},{startPos[1]}]:{curWord}");
                }
                
                if(depth > 0) {
                    foreach (int[] vec in this.NeighborVectors) {
                        this.CharGraph[curPos[0],curPos[1]].Visited = true;
                        PossWords(new int[] {curPos[0]+vec[0], curPos[1]+vec[1]},curWord + this.CharGraph[curPos[0],curPos[1]].Val, depth-1, possWords,startPos);
                        this.CharGraph[curPos[0],curPos[1]].Visited = false;
                    }
                }
            }
        }

        public int[] GetPosFromIndex(int index) {
            return new int[] {index / this.Height, index % this.Width };
        }

        public bool ValidPos(int rowPos, int colPos) {
            return rowPos >= 0 && rowPos < this.Height && colPos >= 0 && colPos < this.Width && this.CharGraph[rowPos,colPos].Visited == false;
        }

        public override string ToString()
        {
            string ret = "";

            for(int r = 0; r < this.Height; r++) {
                for(int c = 0; c < this.Width; c++) {
                    ret += this.CharGraph[r,c].Val + " ";
                }
                ret += "\n";
            }

            return ret;
        }

        public static void Main(String[] args) {
            String input = Console.ReadLine();
            WorldPuzzleSolver wp = new WorldPuzzleSolver(input,4,4);

            List<String> possWords = wp.GenWords();
            
            foreach (String word in possWords) {
                Console.WriteLine(word);
            }
        }
    }
}
