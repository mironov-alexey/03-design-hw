namespace _03_design_hw
{
    public class Word
    {
        public Word(string word, int frequency)
        {
            WordString = word;
            Frequency = frequency;
        }

        public string WordString{ get; }
        public int Frequency{ get; }
    }
}