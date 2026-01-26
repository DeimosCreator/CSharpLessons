using System;

namespace LibraryManagement
{
    public class LibraryItem
    {
        private string title;
        private string creator;
        private bool isAvailable;

        public string Title { get; set; }
        public string Creator { get; set; }
        public bool IsAvailable { get; }

        public void Borrow()
        {
            isAvailable = false;
        }

        public void Return()
        {
            isAvailable = true;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Creator: {Creator}");
            Console.WriteLine($"Available: {(IsAvailable ? "Yes" : "No")}");
        }
    }

    public class Book : LibraryItem
    {
        private int pageCount;

        public int PageCount { get; set; }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Page Count: {PageCount}");
        }
    }

    public class Magazine : LibraryItem
    {
        private int issueNumber;

        public int IssueNumber { get; set; }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Issue Number: {IssueNumber}");
        }
    }

    public class Movie : LibraryItem
    {
        private int duration;
        public int Duration { get; set; }


        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Duration: {Duration} minutes");
        }
    }
}