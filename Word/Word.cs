﻿namespace _03_design_hw.Word
{
    public class Word
    {
        public string WordString{ get; }
        public int Frequency{ get; }

        public Word(string word, int frequency)
        {
            WordString = word;
            Frequency = frequency;
        }
    }
}
