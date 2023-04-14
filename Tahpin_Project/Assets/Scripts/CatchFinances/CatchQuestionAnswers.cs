using System;
using System.Collections.Generic;

[Serializable]
public class CatchQuestionAnswers
{
    public List<CatchQuestionFormat> Question;
    public string[] Answers;
    public int CorrectSlotOne;
    public int CorrectSlotTwo;
    public int CorrectSlotThree;

    public string CombineQuestionText()
    {
        string fullQuestion = "";
        foreach (CatchQuestionFormat word in Question)
        {
            fullQuestion = fullQuestion + word.Text;
        }

        return fullQuestion;
    }
}
