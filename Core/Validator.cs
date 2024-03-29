﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core;

public class Validator
{
    private readonly IList<ValidationResult> _errors;

    public Validator(object subject)
    {
        _errors = new List<ValidationResult>();
        var context = new ValidationContext(subject);
        System.ComponentModel.DataAnnotations.Validator.TryValidateObject(subject, context, _errors, true);
    }

    public IEnumerable<string> Errors => _errors.Select(o => o.ErrorMessage ?? "Unknown error");
    private bool HasErrors => _errors.Count > 0;
    public bool IsValid => !HasErrors;
}