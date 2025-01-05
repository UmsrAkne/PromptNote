using System.Collections.Generic;

namespace PromptNote.Models
{
    public static class PromptMerger
    {
        /// <summary>
        /// ２つの Prompt のリストを、カスタムロジックに従ってマージし、新しいリストを返します。
        /// </summary>
        /// <param name="basePrompts">既存のリスト。</param>
        /// <param name="additionalPrompts">既存のリストに情報を加えるための新しいリスト</param>
        /// <returns>引数に入力されたリストをマージしたリスト。</returns>
        public static List<Prompt> MergePrompts(List<Prompt> basePrompts, List<Prompt> additionalPrompts)
        {
            var list = new List<Prompt>();
            for (var i = 0; i < additionalPrompts.Count; i++)
            {
                Prompt bp = null;
                if (basePrompts.Count > i)
                {
                    bp = basePrompts[i];
                }

                var ap = additionalPrompts[i];
                list.Add(ap);

                if (bp == null || bp.Phrase == ap.Phrase)
                {
                    continue;
                }

                list.Add(bp);
            }

            return list;
        }
    }
}