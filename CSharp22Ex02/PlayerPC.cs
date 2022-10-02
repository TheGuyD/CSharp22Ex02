using System;
using System.Collections.Generic;

namespace CSharp22Ex02
{
    public class PlayerPc
    {
        private int m_Score = 0;
        private int[] m_ArrayOfPcMoves = new int[0];
        private readonly int r_ColumnsOfResultMatrix = 0;

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        // Creating an Array equivalent to size of matrix (cols*rows), and feeling it with number of indexes. 
        // Indexes of this array - this is potential moves of the PC  
        public PlayerPc(int i_Rows, int i_Cols)
        {
            m_ArrayOfPcMoves = new int[i_Rows * i_Cols];

            for(int i = 0; i < m_ArrayOfPcMoves.Length; i++)
            {
                m_ArrayOfPcMoves[i] = i;
            }

            r_ColumnsOfResultMatrix =i_Cols;
        }

        // After succeed move of player or PC, Need to update an array of potential moves of the PC
        public void UpdateArrayOfPcMoves( int[] i_PrevSucceedMoves)
        {
            int indexForDeleteFirst = r_ColumnsOfResultMatrix * i_PrevSucceedMoves[0] + i_PrevSucceedMoves[1];
            int indexForDeleteSecond = r_ColumnsOfResultMatrix * i_PrevSucceedMoves[2] + i_PrevSucceedMoves[3];

            DeleteFromArrayOfPcMove(indexForDeleteFirst);
            DeleteFromArrayOfPcMove(indexForDeleteSecond);
        }

        // Deleting used index 
        public void DeleteFromArrayOfPcMove(int i_IndexForDelete)
        {
            int numIdx = Array.IndexOf(m_ArrayOfPcMoves, i_IndexForDelete);
            List<int> tmp = new List<int>(m_ArrayOfPcMoves);
            tmp.RemoveAt(numIdx);
            m_ArrayOfPcMoves = tmp.ToArray();
        }

        // Logic of PC moves: 
        // Random choose index from Array of moves -> random choose second index (if index is same, repeat step 2) ->
        // -> transformation indexes ti coordinates of matrix
        public int[] PcMakeMove ()
        {
            Random rand = new Random();
            int[] pcMovesCoordinates = new int[4];
            int firstChooseInd = rand.Next(0, m_ArrayOfPcMoves.Length - 1);
            int secondChooseInd = 0;

            if(m_ArrayOfPcMoves.Length == 2)
            {
                firstChooseInd = 0;
                secondChooseInd = 1;
            }
            while (m_ArrayOfPcMoves.Length > 2)
            {
                secondChooseInd = rand.Next(0, m_ArrayOfPcMoves.Length - 1);
                if (secondChooseInd != firstChooseInd) { break; }
            }

            // Transformation to coordinate
            pcMovesCoordinates[0] = m_ArrayOfPcMoves[firstChooseInd] / r_ColumnsOfResultMatrix;
            pcMovesCoordinates[1] = m_ArrayOfPcMoves[firstChooseInd] % r_ColumnsOfResultMatrix;
            pcMovesCoordinates[2] = m_ArrayOfPcMoves[secondChooseInd] / r_ColumnsOfResultMatrix;
            pcMovesCoordinates[3] = m_ArrayOfPcMoves[secondChooseInd]% r_ColumnsOfResultMatrix;

            return pcMovesCoordinates;
        }
    }
}
 