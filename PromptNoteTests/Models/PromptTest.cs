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

        [Test]
        [TestCase("test", PromptType.Normal)]
        [TestCase("<lora:test>", PromptType.Lora)]
        [TestCase("{p1|p2}", PromptType.DynamicPrompt)]
        [TestCase("{p1|p2} p3", PromptType.DynamicPrompt)]
        [TestCase("\r", PromptType.LineBreak)]
        [TestCase("\r\n", PromptType.LineBreak)]
        [TestCase("\n", PromptType.LineBreak)]
        public void TypeDetectionTest(string phrase, PromptType expectedType)
        {
            var actual = new Prompt(phrase).Type;
            Assert.That(actual, Is.EqualTo(expectedType));
        }

        [Test]
        [TestCase("{p1|p2}", "{p1|p2}")] // 通常ケース。
        [TestCase("{p1|p2|p3}", "{p1|p2|p3}")] // 変数３つ。
        [TestCase("{p1|p2|P3}", "{p1|p2|p3}")] // 変数３つ、大文字混在。小文字に変換されるか？
        [TestCase("{p1|p3|p2}", "{p1|p2|p3}")] // 変数３つ、順番違い。ソートされて出力されるか？
        [TestCase("{p1|p2} p3", "{p1|p2} p3")] // 通常ケース、末尾に定数ワードつき。
        public void FormatDynamicPromptTest(string phrase, string expectedPhrase)
        {
            var actual = new Prompt(phrase).Phrase.Value;
            Assert.That(actual, Is.EqualTo(expectedPhrase));
        }
    }
}