using System;
using System.Linq;
using Xamarin.Forms;

namespace aclara_meters.Behaviors
{
    public class CommentsLengthValidatorBehavior : Behavior<BorderlessEntry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(BorderlessEntry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(BorderlessEntry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (BorderlessEntry)sender;


            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
             
                if (entry.Text != null && entry.Text.Length > this.MaxLength)
                {
                    string entryText = entry.Text;
                    entry.TextChanged -= OnEntryTextChanged;
                    entry.Text = e.OldTextValue;
                    entry.TextChanged += OnEntryTextChanged;
                }

            }

        }
    }
}
