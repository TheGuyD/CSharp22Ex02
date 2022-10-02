namespace CSharp22Ex02

{
    
     internal class Player
    {
        private string m_Name = string.Empty;
        private int m_Score = 0;
       
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

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

        public Player(string i_Name)
        {
            m_Name = i_Name;
        }

        public static bool IsNameValid(string i_Name)
        {
            bool isNameValid = true;

            foreach(char c in i_Name )
            {
                if(!char.IsLetter(c))
                {
                    isNameValid = false;
                    break;
                }
            }

            return isNameValid;
        }
    }
}
