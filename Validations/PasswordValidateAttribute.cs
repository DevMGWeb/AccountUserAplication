using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SistemaBiblioteca.Validations
{
    public class PasswordValidateAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int minCharacter;
        private readonly bool lowerCase;
        private readonly bool upperCase;
        private readonly bool digistNumeric;
        private readonly bool specialCharacter;

        public PasswordValidateAttribute(int MinCharacter, bool LowerCase = true, bool UpperCase = true, 
            bool DigistNumeric = true, bool SpecialCharacter = false) : 
            base("El formato {0} es incorrecto")
        {
            minCharacter = MinCharacter;
            lowerCase = LowerCase;
            upperCase = UpperCase;
            digistNumeric = DigistNumeric;
            specialCharacter = SpecialCharacter;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule passwordRule = new ModelClientValidationRule();
            passwordRule.ErrorMessage = this.ErrorMessageString;
            passwordRule.ValidationType = "memberpasswordvalidation"; // Type name must be all lowercase! camelCaseBadHere.
            yield return passwordRule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool PasswordValid = false;
            string reason = String.Empty;
            string messageFeaturesValid = String.Empty;
            string Password = value == null ? String.Empty : value.ToString();
            if (String.IsNullOrEmpty(Password) || Password.Length < minCharacter)
            {
                reason = $"Tu nuevo password debe tener al menos {minCharacter} caracteres. ";
            }
            else
            {
                List<string> patterns = new List<string>();
                if (lowerCase)
                {
                    patterns.Add(@"[a-z]"); // lowercase
                    messageFeaturesValid = addFeaturesValid(messageFeaturesValid, "una minuscula", upperCase);
                }

                if (upperCase)
                {
                    patterns.Add(@"[A-Z]"); // uppercase
                    messageFeaturesValid = addFeaturesValid(messageFeaturesValid, "una mayuscula", digistNumeric);
                }

                if (digistNumeric)
                {
                    patterns.Add(@"[0-9]"); // digits  4
                    messageFeaturesValid = addFeaturesValid(messageFeaturesValid, "un numero", specialCharacter);
                }

                if (specialCharacter)
                {
                    patterns.Add(@"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\] ]"); // special symbols
                    messageFeaturesValid += "un caracter especial";
                }

                // count type of different chars in password  
                foreach (string p in patterns)
                {
                    if (!Regex.IsMatch(Password, p))
                    {
                        reason += $"Tu contraseña debe tener al menos {messageFeaturesValid}." ;
                        break;
                    }
                }

                if(reason == string.Empty)
                {
                    PasswordValid = true;
                }
            }

            if (PasswordValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(reason);
            }
        }

        private string addFeaturesValid(string featureValid, string messageFeatureValid, bool nextFlag)
        {
            featureValid += messageFeatureValid;
            featureValid = addSeparationFeatures(featureValid, nextFlag);
            
            return featureValid;
        }

        private string addSeparationFeatures(string messageFeatureValid, bool nextFlag)
        {
            if (!string.IsNullOrEmpty(messageFeatureValid) && nextFlag)
            {
                messageFeatureValid += ", ";
            }

            return messageFeatureValid;
        }
    }
}
