using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CoderPadWordCount
{
    public class WordCountService
    {
        /// <summary>
        /// The objective of this method is to find the topmost frequently
        /// found words in the parameter "text", and return them in 
        /// descending order of frequency.
        /// Make your comparisons case-insensitive and strip punctuation.
        /// The result should be a list of strings all converted to 
        /// lowercase, and the length of the list returned should be equal
        /// to the value provided in the "top" parameter.
        /// </summary>
        /// <param name="text">A string containing text from an external 
        /// source (e.g. Shakespeare's Hamlet)</param>
        /// <param name="top">A positive integer representing the number of
        /// topmost words to return</param>
        /// <returns>A list of strings containing the top most frequently 
        /// found words, in descending order</returns>
        public List<string> CountWords(string text, int top)
        {
            var actualWords = Regex.Split(text.ToLower(), @"\W+")
                .GroupBy(s => s)
                .OrderByDescending(g => g.Count())
                .Select(s => s.Key)
                .Take(top)
                .ToList();
            return actualWords;
        }
    }
    [TestFixture]
    public class WordCountService_Should
    {
        [Test]
        public void ReturnTrueGivenHamletText()
        {
            // Arrange
            const int top = 10;
            var expectedWords = new List<string>() { "the", "and", "to", "of", "i", "you", "a", "my", "hamlet", "in" };
            var wordCounter = new WordCountService();
            var text = GetText();

            // Act
            var mostFrequentWords = wordCounter.CountWords(text, top);

            // Assert
            CollectionAssert.AreEqual(expectedWords, mostFrequentWords,
                "Incorrect words received:\nexpected: {0}\nreceived {1}", string.Join(", ", expectedWords), string.Join(", ", mostFrequentWords));
        }

        private string GetText()
        {
            string text = string.Empty;
            const string textUri = "https://raw.githubusercontent.com/hautelook/halo-word-count/master/hamlet.txt";
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = client.GetAsync(textUri).Result;
                if (result.IsSuccessStatusCode)
                {
                    text = result.Content.ReadAsStringAsync().Result;
                }
            }
            return text;
        }
    }
}
