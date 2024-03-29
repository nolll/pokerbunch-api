﻿namespace Api.Urls;

public abstract class Url
{
    protected abstract string Input { get; }
    public string Relative => Input.ToLower();
    public string Absolute(string host) => $"https://{host}{Relative}";
}
