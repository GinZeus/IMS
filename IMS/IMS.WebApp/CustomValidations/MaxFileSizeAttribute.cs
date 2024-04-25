using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.CustomValidations
{
	public class MaxFileSizeAttribute : ValidationAttribute, IClientModelValidator
	{
		private readonly int _maxFileSize;
		public MaxFileSizeAttribute(int maxFileSize)
		{
			_maxFileSize = maxFileSize;
		}

		public void AddValidation(ClientModelValidationContext context)
		{
			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, "data-val-filesize", $"Maximum allowed file size is {_maxFileSize} bytes");
		}

		private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
		{
			if (attributes.ContainsKey(key))
			{
				return false;
			}

			attributes.Add(key, value);
			return true;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			IFormFile? file = value as IFormFile;
			if (file != null)
			{
				if (file.Length > _maxFileSize)
					return new ValidationResult($"Maximum allowed file size is {_maxFileSize} bytes");

				return ValidationResult.Success;
			}

			return new ValidationResult($"File is null");
		}
	}
}
