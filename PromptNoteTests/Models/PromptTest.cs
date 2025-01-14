using PromptNote.Models;

namespace PromptNoteTests.Models
{
    [TestFixture]
    public class PromptTest
    {
        [Test]
        [TestCase("test", PromptType.Normal, "test")]
        [TestCase("<lora:test>", PromptType.Lora, "<lora:test:1>")]
        public void ToStringTest(string phrase, PromptType type, string expected)
        {
            var actual = new Prompt() { Phrase = new Phrase() { Value = phrase, }, Type = type, }.ToString();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}