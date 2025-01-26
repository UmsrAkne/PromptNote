namespace PromptNote.Models
{
    public enum PromptType
    {
        /// <summary>
        /// 通常のプロンプト
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Loraを表します。
        /// </summary>
        Lora = 1,

        /// <summary>
        /// 改行を表します。
        /// </summary>
        LineBreak = 2,

        /// <summary>
        /// ダイナミックプロンプト。
        /// </summary>
        DynamicPrompt = 3,

        /// <summary>
        /// Phrase が空のプロンプトです。
        /// </summary>
        Empty = 4,
    }
}