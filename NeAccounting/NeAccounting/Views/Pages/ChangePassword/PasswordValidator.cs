using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasswordStrengthFinder
{
    public enum PasswordStrength
    {
        UpperCase=10,LowerCase=9,Symbol=11,Digit=8,Length=12,NotCommon=50
    }

 
    internal class PasswordValidator
    {
        Dictionary<PasswordStrength,bool> Conditions;
        Dictionary<PasswordStrength, string> Suggestions;
        public PasswordValidator()
        {
            Conditions = new Dictionary<PasswordStrength,bool>();
            Suggestions = new Dictionary<PasswordStrength,string>();
            Suggestions.Add(PasswordStrength.UpperCase, "Add Uppercase Character");
            Suggestions.Add(PasswordStrength.LowerCase, "Add Lowercase Character");
            Suggestions.Add(PasswordStrength.Symbol, "Add Special Character");
            Suggestions.Add(PasswordStrength.Digit, "Add Number");
            Suggestions.Add(PasswordStrength.Length, "Password must have a minimum length of 8");
            Suggestions.Add(PasswordStrength.NotCommon, "Password is in common list. Try a complicated password");
        }

        public bool IsStrong(string password,out int Value)
        {
            Value = 0;
            setPasswordStrengths(password);
            return checkPasswordScore(ref Value);
        }

        private void setPasswordStrengths(string password)
        {
            Conditions.Clear();
            setPasswordStrength(PasswordStrength.Length, password.Length > 8);
            setPasswordStrength(PasswordStrength.UpperCase, password.Any(char.IsUpper));
            setPasswordStrength(PasswordStrength.LowerCase, password.Any(char.IsLower));
            setPasswordStrength(PasswordStrength.Symbol, password.Any(c=>!char.IsLetterOrDigit(c)));
            setPasswordStrength(PasswordStrength.Digit, password.Any(char.IsDigit));
            setPasswordStrength(PasswordStrength.NotCommon, !passwordExists(password));
        }

        private void setPasswordStrength(PasswordStrength strength,bool IsSatisfied)
        {
            Conditions[strength] = IsSatisfied;
        }

        private bool passwordExists(string password)
        {
            IEnumerable<string> lines= File.ReadLines("millionpassword.txt");
            foreach (string line in lines)
                if (password == line)
                    return true; // and stop reading lines

            return false;
        }

        private bool checkPasswordScore(ref int Value)
        {
            int passwordScore = 0;
            foreach(var strength in Conditions)
            {
                if (strength.Value)
                {
                    passwordScore += (int)strength.Key;
                }
            }

            if(passwordScore < 50)
            {
                Value = 10;
                return false;
            }
            
            if(passwordScore >= 50 && passwordScore < 60)
            {
                Value = 25;
                return false;
            }

            if (passwordScore >= 60 && passwordScore < 70)
            {
                Value = 50;
                return false;
            }

            if (passwordScore >= 70 && passwordScore < 80)
            {
                Value = 75;
                return false;
            }

            if (passwordScore >= 80 && passwordScore <= 92)
            {
                Value = 100;
                return true;
            }


            return true;
        }

    }
}