using System;

namespace PuzzleHunt.Web.Models
{
    public class HintModel
    {
        public int Id { get; private set; }
        public int Order { get; private set; }
        public string Content { get; private set; }

        public HintModel(int id, int order, string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            Id = id;
            Order = order;
            Content = content;
        }
    }
}