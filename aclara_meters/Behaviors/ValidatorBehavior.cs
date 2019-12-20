﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Globalization;

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
            bool IsValid = Regex.IsMatch(e.NewTextValue, fieldRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
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
            bool IsValid = Regex.IsMatch(e.NewTextValue, fieldRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            ((BorderlessEntry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(BorderlessEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
    public class CommentsLengthValidatorBehavior : Behavior<BorderlessEntry>
    {
        public int MaxLength { get; set; }
        public int MinLength { get; set; }


        protected override void OnAttachedTo(BorderlessEntry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
            bindable.Completed += OnEntryCompleted;
        }

        protected override void OnDetachingFrom(BorderlessEntry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
            bindable.Completed -= OnEntryCompleted;
        }

        private void OnEntryCompleted(object sender, EventArgs e)
        {
            var entry = (BorderlessEntry)sender;

            if (entry.Text != null
                 && entry.Text.Length < this.MinLength)
            {
                entry.TextColor = Color.Red;              
            }
            else
            {
                entry.TextColor = Color.Default;
            }
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (BorderlessEntry)sender;


            if (!string.IsNullOrWhiteSpace(e.NewTextValue) && entry.Text != null 
                && entry.Text.Length > this.MaxLength )
            {

                entry.TextChanged -= OnEntryTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnEntryTextChanged;
            }

        }
       
    }
    public class EntryLengthValidatorBehavior : Behavior<BorderlessEntry>
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
                bool isValid = e.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                ((BorderlessEntry)sender).Text = isValid ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);

                if (isValid)
                {
                    if (entry.Text != null && entry.Text.Length > this.MaxLength)
                    {                        
                        entry.TextChanged -= OnEntryTextChanged;
                        entry.Text = e.OldTextValue;
                        entry.TextChanged += OnEntryTextChanged;
                    }
                }
            }

        }
    }
    
}
