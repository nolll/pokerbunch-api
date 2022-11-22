from dataclasses import replace
from operator import concat


def readLines():
    with open("sqlserver.sql", "r") as inputFile:
        inputLines = inputFile.readlines()
        return inputLines


def writeLines(lines):
    with open('postgresql.sql', 'w') as outputFile:
        outputFile.writelines(lines)


def isInsertStatement(line):
    if (line.startswith('INSERT')):
        return True
    return False


def migrateLine(line):
    output = line.replace('CAST(N', '')
    output = output.replace(' AS Date)', '')
    output = output.replace(' AS DateTime)', '')
    output = output.replace('N\'', '\'')
    output = output.replace(')', ');')
    output = output.replace('); VALUES', ') VALUES')
    output = output.replace(
        '[dbo].[Game] ([GameId], [Date], [Timestamp], [Status], [HomegameId], [LocationId])',
        'INTO pb_cashgame (cashgame_id, date, timestamp, status, bunch_id, location_id)')
    output = output.replace(
        '[dbo].[CashgameCheckpoint] ([CheckpointId], [GameId], [PlayerId], [Type], [Amount], [Stack], [Timestamp])',
        'INTO pb_cashgame_checkpoint (checkpoint_id, cashgame_id, player_id, type, amount, stack, timestamp)')
    output = output.replace(
        '[dbo].[Player] ([PlayerId], [HomegameId], [UserId], [RoleId], [Approved], [PlayerName], [Color])',
        'INTO pb_player (player_id, bunch_id, user_id, role_id, approved, player_name, color)')
    output = output.replace(
        '[dbo].[App] ([Id], [AppKey], [Name], [UserId])',
        'INTO pb_app (app_id, app_key, name, user_id)')
    output = output.replace(
        '[dbo].[CashgameComment] ([GameId], [CommentId])',
        'INTO pb_cashgame_comment (cashgame_id, comment_id)')
    output = output.replace(
        '[dbo].[Comment] ([CommentId], [PlayerId], [Date], [CommentText])',
        'INTO pb_comment (comment_id, player_id, date, comment_text)')
    output = output.replace(
        '[dbo].[Event] ([EventId], [Name], [BunchId])',
        'INTO pb_event (event_id, name, bunch_id)')
    output = output.replace(
        '[dbo].[EventCashgame] ([EventId], [GameId])',
        'INTO pb_event_cashgame (event_id, cashgame_id)')
    output = output.replace(
        '[dbo].[Homegame] ([HomegameId], [Name], [DisplayName], [Description], [Timezone], [DefaultBuyin], [Currency], [CurrencyLayout], [CashgamesEnabled], [TournamentsEnabled], [VideosEnabled], [HouseRules])',
        'INTO pb_bunch (bunch_id, name, display_name, description, timezone, default_buyin, currency, currency_layout, cashgames_enabled, tournaments_enabled, videos_enabled, House_rules)')
    output = output.replace(
        '[dbo].[Location] ([Id], [Name], [BunchId])',
        'INTO pb_location (location_id, name, bunch_id)')
    output = output.replace(
        '[dbo].[Role] ([RoleId], [RoleName])',
        'INTO pb_role (role_id, role_name)')
    output = output.replace(
        '[dbo].[User] ([UserId], [Token], [UserName], [Password], [Salt], [RoleId], [RealName], [DisplayName], [Email])',
        'INTO pb_user (user_id, token, user_name, password, salt, role_id, real_name, display_name, email)')
    output = output.replace(
        '[dbo].[UserSharing] ([UserId], [ServiceName])',
        'INTO pb_user_sharing (user_id, service_name)')
    output = output.replace(
        '[dbo].[UserTwitter] ([UserId], [TwitterName], [Key], [Secret])',
        'INTO pb_user_twitter (user_id, twitter_name, key, secret)')
    output = output.replace(
        '[dbo].[Video] ([VideoId], [HomegameId], [Date], [Thumbnail], [Length], [Width], [Height], [Source], [Type], [Hidden])',
        'INTO pb_video (video_id, bunch_id, date, thumbnail, length, width, height, source, type, hidden)')
    return output


outputLines = map(migrateLine, filter(isInsertStatement, readLines()))
writeLines(outputLines)
