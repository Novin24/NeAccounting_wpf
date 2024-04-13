using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeAccounting.Helpers.Extention
{
    public enum PasswordStrength
    {
        UpperCase = 10, LowerCase = 9, Symbol = 11, Digit = 8, Length = 12, NotCommon = 50
    }


    internal class PasswordValidator
    {
        Dictionary<PasswordStrength, bool> Conditions;
        Dictionary<PasswordStrength, string> Suggestions;
        public PasswordValidator()
        {
            Conditions = [];
            Suggestions = new Dictionary<PasswordStrength, string>
            {
                { PasswordStrength.UpperCase, "حروف بزرگ اضافه کنید" },
                { PasswordStrength.LowerCase, "حروف کوچک اضافه کنید" },
                { PasswordStrength.Symbol, "اضافه کردن کاراکترهای خاص مثل !@#$%^&*()" },
                { PasswordStrength.Digit, "اضافه کردن عدد" },
                { PasswordStrength.Length, "رمز عبور باید حداقل 8 کاراکتر داشته باشد" },
                { PasswordStrength.NotCommon, "رمز عبور در لیست رایج است. رمز عبور پیچیده را امتحان کنید" }
            };
        }

        public (bool isStrong, int perc) IsStrong(string password, out string message)
        {
            message = string.Empty;
            setPasswordStrengths(password);
            return checkPasswordScore(ref message);
        }

        private void setPasswordStrengths(string password)
        {
            Conditions.Clear();
            setPasswordStrength(PasswordStrength.Length, password.Length > 8);
            setPasswordStrength(PasswordStrength.UpperCase, password.Any(char.IsUpper));
            setPasswordStrength(PasswordStrength.LowerCase, password.Any(char.IsLower));
            setPasswordStrength(PasswordStrength.Symbol, password.Any(c => !char.IsLetterOrDigit(c)));
            setPasswordStrength(PasswordStrength.Digit, password.Any(char.IsDigit));
            setPasswordStrength(PasswordStrength.NotCommon, !passwordExists(password));
        }

        private void setPasswordStrength(PasswordStrength strength, bool IsSatisfied)
        {
            Conditions[strength] = IsSatisfied;
        }

        private bool passwordExists(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return true;
            }
            IEnumerable<string> lines = File.ReadLines(@"Required\Password\millionpassword.txt");
            foreach (string line in lines)
                if (password == line)
                    return true; // and stop reading lines

            return false;
        }

        private (bool isStrong, int perc) checkPasswordScore(ref string message)
        {
            int passwordScore = 0;
            foreach (var strength in Conditions)
            {
                if (strength.Value)
                {
                    passwordScore += (int)strength.Key;
                }
            }
            if (passwordScore == 0 )
            {
                message = "وارد کردن پسورد الزامیست!!!";
                return new(false, 2);
            }

            if (passwordScore < 50)
            {
                message = "رمز عبور خیلی رایج است. احتمالاً به راحتی شکسته می شود.";
                return new(false, 10);
            }

            if (passwordScore >= 50 && passwordScore < 60)
            {
                message = ".پسورد خیلی ضعیفه. " + additionalSuggestions();
                return new(false, 20);
            }

            if (passwordScore >= 60 && passwordScore < 70)
            {
                message = "رمز عبور ضعیف است. " + additionalSuggestions();
                return new(false, 40);
            }

            if (passwordScore >= 70 && passwordScore < 80)
            {
                message = "رمز عبور متوسط است. " + additionalSuggestions();
                return new(false, 60);
            }

            if (passwordScore >= 80 && passwordScore <= 92)
            {
                message = "رمز عبور قوی است." + additionalSuggestions();
                return new(true, 80);
            }

            message = "رمز عبور به اندازه کافی قوی است.";
            return new(true, 100);
        }

        private string additionalSuggestions()
        {
            string additionalSuggestions = string.Empty;
            foreach (var strength in Conditions)
            {
                if (!strength.Value)
                {
                    additionalSuggestions += "\n" + Suggestions[strength.Key];
                }
            }
            return additionalSuggestions;
        }
    }
}