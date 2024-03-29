﻿using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.PlayerModels;

public class PlayerListItemModel
{
    [JsonPropertyName("id")]
    public string Id { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("userId")]
    public string? UserId { get; }

    [JsonPropertyName("userName")]
    public string? UserName { get; }

    [JsonPropertyName("color")]
    public string? Color { get; }

    public PlayerListItemModel(GetPlayerList.ResultItem r)
    {
        Id = r.Id;
        Name = r.Name;
        UserId = r.UserId;
        UserName = r.UserName;
        Color = r.Color;
    }

    [JsonConstructor]
    public PlayerListItemModel(string id, string name, string userId, string userName, string color)
    {
        Id = id;
        Name = name;
        UserId = userId;
        UserName = userName;
        Color = color;
    }
}