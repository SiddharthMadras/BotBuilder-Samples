namespace CreateNewConversationBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class Survey
    {
        [Prompt("Hello... What is your issue related with?")]
        public string Name { get; set; }

        [Prompt("How many days, since your facing this?")]
        public int YearsCoding { get; set; }

        [Prompt("Please give us your contact number?")]
        public string Language { get; set; }
    }
}
