namespace MultiDialogsBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class HotelsQuery
    {
        [Prompt("Please enter your {&}")]
        public string Destination { get; set; }
        
        [Prompt("We understand your concern. For how many days your facing this issue {&}?")]
        public DateTime CheckIn { get; set; }

        [Numeric(1, int.MaxValue)]
        [Prompt("How many {&} resources, you'll need?")]
        public int Nights { get; set; }
    }
}
