using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.Support
{
    public class TypingBehavior : Behavior<Control>
    {

        private static readonly Regex _charactersToIgnore = new Regex(@"[^a-zA-Z0-9 \.\-\b]");

        protected override void OnAttached()
        {
            Messenger.Default.Register<TextCompositionEventArgs>(this, GetInput);
        }

        protected override void OnDetaching()
        {
            Messenger.Default.Unregister<TextCompositionEventArgs>(this);
        }

        private void GetInput(TextCompositionEventArgs textCompositionEventArgs)
        {
            var keyboardFocusElement = Keyboard.FocusedElement;
            if (keyboardFocusElement is TextBoxBase || keyboardFocusElement is ComboBox)
            {
                return;
            }

            var textVal = textCompositionEventArgs.Text;
            var b = new StringBuilder(TextValue);

            var output = _charactersToIgnore.Replace(textVal, "");
            b.Append(output);
            TextValue = EvaluateOutBackspaces(b).ToString();
            textCompositionEventArgs.Handled = true;
        }


        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof (string), typeof (TypingBehavior), new PropertyMetadata(string.Empty));

        public string TextValue
        {
            get { return (string) GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }

    

        private StringBuilder EvaluateOutBackspaces(StringBuilder input)
        {
            var listOfIndexesToRemove = new List<int>();
            for (var i =0; i < input.Length; i++)
            {
                if (input[i] == '\b')
                {
                    if (i > 0)
                    {
                        listOfIndexesToRemove.Add(i-1);
                    }
                    listOfIndexesToRemove.Add(i);
                }
            }

            //removeindexes in reverse order

            input = Enumerable.Reverse(listOfIndexesToRemove).ToArray().Aggregate(input, (current, i) => current.Remove(i, 1));

            return input;
        }


    }

   
}
