--NEXT RELEASE


--6.0
--update game set status = 2 where status = 3

--5.0
--ALTER TABLE `game` DROP COLUMN `Notified`, DROP COLUMN `Duration`, DROP COLUMN `StartTime`, DROP COLUMN `EndTime`;
--ALTER TABLE `homegame` CHANGE COLUMN `CashgamesEnabled` `CashgamesEnabled` BIT NOT NULL DEFAULT b'0' AFTER `CurrencyLayout`, CHANGE COLUMN `TournamentsEnabled` `TournamentsEnabled` BIT NOT NULL DEFAULT b'0' AFTER `CashgamesEnabled`, CHANGE COLUMN `VideosEnabled` `VideosEnabled` BIT NOT NULL DEFAULT b'0' AFTER `TournamentsEnabled`;
--ALTER TABLE `homegame` CHANGE COLUMN `CashgamesEnabled` `CashgamesEnabled` INT(4) NOT NULL DEFAULT '0' AFTER `CurrencyLayout`, CHANGE COLUMN `TournamentsEnabled` `TournamentsEnabled` INT(4) NOT NULL DEFAULT '0' AFTER `CashgamesEnabled`, CHANGE COLUMN `VideosEnabled` `VideosEnabled` INT(4) NOT NULL DEFAULT '0' AFTER `TournamentsEnabled`;
--UPDATE `homegame` SET `Timezone`='W. Europe Standard Time' WHERE  `HomegameID`=1;
--UPDATE `homegame` SET `Timezone`='W. Europe Standard Time' WHERE  `HomegameID`=3;
--UPDATE `homegame` SET `Timezone`='W. Europe Standard Time' WHERE  `HomegameID`=4;
--UPDATE `homegame` SET `Timezone`='W. Europe Standard Time' WHERE  `HomegameID`=5;
--UPDATE `homegame` SET `Timezone`='W. Europe Standard Time' WHERE  `HomegameID`=6;
--ALTER TABLE `homegame` CHANGE COLUMN `Description` `Description` VARCHAR(50) NULL AFTER `DisplayName`, CHANGE COLUMN `HouseRules` `HouseRules` TEXT NULL AFTER `VideosEnabled`;


--3.0
--ALTER TABLE `game`
--ADD COLUMN `Status` INT(4) NOT NULL DEFAULT '0' AFTER `Timestamp`;
--UPDATE game SET Status = 3 WHERE Published = 1;
--ALTER TABLE `game` DROP COLUMN `Published`;
--CREATE TABLE `cashgamecheckpoint` ( `GameID` INT(4) NOT NULL, `PlayerID` INT(4) NOT NULL, `Type` INT(4) NOT NULL DEFAULT '0', `Stack` INT(4) NOT NULL DEFAULT '0', `Timestamp` DATETIME NOT NULL, PRIMARY KEY (`GameID`, `PlayerID`) ) COLLATE='utf8_general_ci' ENGINE=MyISAM;
--ALTER TABLE `cashgamecheckpoint` ADD COLUMN `CheckpointID` INT(4) NOT NULL AUTO_INCREMENT FIRST, DROP PRIMARY KEY, ADD PRIMARY KEY (`CheckpointID`);
--UPDATE `game` SET `Location`='Malmköping' WHERE  `GameID`=8 LIMIT 1;
--UPDATE game SET location = 'Björn' WHERE location = '';
--UPDATE cashgameresult SET buyin = 200, cashout = cashout + 200 WHERE buyin = 0 AND cashout > 0;
--UPDATE cashgameresult SET buyin = 200, cashout = cashout + 200 WHERE buyin = 0 AND cashout >= -200;
--UPDATE cashgameresult SET buyin = 400, cashout = cashout + 400 WHERE buyin = 0 AND cashout >= -400;
--UPDATE cashgameresult SET buyin = 600, cashout = cashout + 600 WHERE buyin = 0 AND cashout >= -600;
--UPDATE cashgameresult SET buyin = 800, cashout = cashout + 800 WHERE buyin = 0 AND cashout >= -800;
--UPDATE cashgameresult SET buyin = 1000, cashout = cashout + 1000 WHERE buyin = 0 AND cashout >= -1000;
--UPDATE cashgameresult SET buyin = 1200, cashout = cashout + 1200 WHERE buyin = 0 AND cashout >= -1200;
--UPDATE cashgameresult SET buyin = 1400, cashout = cashout + 1400 WHERE buyin = 0 AND cashout >= -1400;
--UPDATE cashgameresult SET buyin = 1600, cashout = cashout + 1600 WHERE buyin = 0 AND cashout >= -1600;
--ALTER TABLE `game` CHANGE COLUMN `PreviouslyPublished` `Notified` TINYINT(1) NOT NULL DEFAULT '0' AFTER `Status`;
--ALTER TABLE `game` ADD COLUMN `StartTime` DATETIME NULL AFTER `Notified`;
--ALTER TABLE `game` ADD COLUMN `EndTime` DATETIME NULL AFTER `StartTime`;
--UPDATE game SET starttime = concat(date, ' 18:30:00') WHERE status = 3 AND starttime is null;
--UPDATE game SET endtime = DATE_ADD(starttime, INTERVAL duration MINUTE) WHERE status = 3 AND endtime is null;
--ALTER TABLE `cashgamecheckpoint` ADD COLUMN `Amount` INT(4) NOT NULL DEFAULT '0' AFTER `Type`;
--UPDATE `comment` SET `Date` = CONVERT_TZ(`Date`, 'CET', 'UTC');
--UPDATE `cashgamecheckpoint` SET `Timestamp` = CONVERT_TZ(`Timestamp`, 'CET', 'UTC');
--UPDATE `game` SET `Timestamp` = CONVERT_TZ(`Timestamp`, 'CET', 'UTC'), `StartTime` = CONVERT_TZ(`StartTime`, 'CET', 'UTC'), `EndTime` = CONVERT_TZ(`EndTime`, 'CET', 'UTC');

--2.6
--ALTER TABLE `user` ADD COLUMN `RealName` VARCHAR(50) NULL DEFAULT NULL AFTER `LastName`;
--UPDATE user SET RealName = CONCAT(FirstName, ' ', LastName);
--ALTER TABLE `user` DROP COLUMN `FirstName`, DROP COLUMN `LastName`;