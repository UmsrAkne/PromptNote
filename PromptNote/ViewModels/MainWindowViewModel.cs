﻿using System.Collections.Generic;
using System.Diagnostics;
using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public TextWrapper TextWrapper { get; set; } = new ();

        public PromptsViewModel PromptsViewModel { get; set; } = new ();

        public MainWindowViewModel()
        {
            SetDummies();
        }

        [Conditional("DEBUG")]
        private void SetDummies()
        {
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test1", });

            PromptsViewModel.Prompts.Add(new Prompt()
                { Phrase = "test2", Tags = new List<string>() { "Tag1", }, });

            PromptsViewModel.Prompts.Add(new Prompt()
                { Phrase = "test3", Tags = new List<string>() { "Tag1", "Tag2", }, });

            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test4", });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test5longLongLongLongLongLongLongText", });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test6", });
        }
    }
}