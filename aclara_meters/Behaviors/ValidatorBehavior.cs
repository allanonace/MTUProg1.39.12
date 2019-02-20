using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace aclara_meters.Behaviors
{
    public class EntryValidatorBehavior : Behavior<BorderlessEntry>
    {
        const string fieldRegex = @"^([a-zA-Z0-9_\-]+$)";


        protected override void OnAttachedTo(BorderlessEntry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            IsValid = (Regex.IsMatch(e.NewTextValue, fieldRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((BorderlessEntry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(BorderlessEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
    public class EntryNumericValidatorBehavior : Behavior<BorderlessEntry>
    {
        const string fieldRegex = @"^([0-9]+$)";


        protected override void OnAttachedTo(BorderlessEntry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            IsValid = (Regex.IsMatch(e.NewTextValue, fieldRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((BorderlessEntry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(BorderlessEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
