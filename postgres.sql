DROP TABLE app;
CREATE TABLE app(
	id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	app_key VARCHAR(50) NOT NULL,
	name VARCHAR(50) NOT NULL,
	user_id INT NOT NULL
);

DROP TABLE cashgame_checkpoint;
CREATE TABLE cashgame_checkpoint(
	checkpoint_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	game_id INT NOT NULL,
	player_id INT NOT NULL,
	type INT DEFAULT 0 NOT NULL,
	amount INT DEFAULT 0 NOT NULL,
	stack INT DEFAULT 0 NOT NULL,
	timestamp TIMESTAMP NOT NULL
);

DROP TABLE cashgame_comment;
CREATE TABLE cashgame_comment(
	game_id INT NOT NULL,
	comment_id INT NOT NULL,
    PRIMARY KEY (game_id, comment_id)
);

-- Maybe something wrong. Got an error:
-- ERROR: relation "comment" already exists
-- SQL state: 42P07
DROP TABLE comment;
CREATE TABLE comment(
	comment_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	player_id INT NOT NULL,
	date TIMESTAMP NOT NULL,
	comment_text VARCHAR(1024) NOT NULL
);

DROP TABLE event;
CREATE TABLE event(
	event_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	bunch_id INT DEFAULT 0 NOT NULL
);

DROP TABLE event_cashgame;
CREATE TABLE event_cashgame(
	event_id INT NOT NULL,
	game_id INT NOT NULL,
    PRIMARY KEY (event_id, game_id)
);

DROP TABLE game;
CREATE TABLE game(
	game_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	date date NOT NULL,
	timestamp TIMESTAMP NOT NULL,
	status INT NOT NULL,
	homegame_id INT NOT NULL,
	location_id INT NOT NULL
);

DROP TABLE homegame;
CREATE TABLE homegame(
	homegame_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	display_name VARCHAR(50) NULL,
	description VARCHAR(50) NULL,
	timezone VARCHAR(50) NOT NULL,
	default_buyin INT NOT NULL,
	currency VARCHAR(3) NOT NULL,
	currency_layout VARCHAR(20) NOT NULL,
	cashgames_enabled bit NOT NULL,
	tournaments_enabled bit NOT NULL,
	videos_enabled bit NOT NULL,
	house_rules text NULL
);

DROP TABLE location;
CREATE TABLE location(
	id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	bunch_id INT NOT NULL
);

DROP TABLE player;
CREATE TABLE player(
	player_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	homegame_id INT NOT NULL,
	user_id INT NULL,
	role_id INT NOT NULL,
	approved bit NOT NULL,
	player_name VARCHAR(50) NULL,
	color VARCHAR(10) NULL
);

DROP TABLE role;
CREATE TABLE role(
	role_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	role_name VARCHAR(50) NOT NULL
);

DROP TABLE tournament;
CREATE TABLE tournament(
	tournament_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	homegame_id INT NOT NULL,
	buyin INT NOT NULL,
	date date NOT NULL,
	duration INT NOT NULL,
	location VARCHAR(50) NOT NULL,
	timestamp TIMESTAMP NOT NULL,
	published bit NOT NULL
);

DROP TABLE tournament_comment;
CREATE TABLE tournament_comment(
	tournament_id INT NOT NULL,
	comment_id INT NOT NULL,
    PRIMARY KEY (tournament_id, comment_id)
);

DROP TABLE tournament_payout;
CREATE TABLE tournament_payout(
	tournament_id INT NOT NULL,
	position INT NOT NULL,
	payout INT NOT NULL,
    PRIMARY KEY (tournament_id, position)
);

DROP TABLE tournament_result;
CREATE TABLE tournament_result(
	tournament_id INT NOT NULL,
	player_id INT NOT NULL,
	position INT NOT NULL,
    PRIMARY KEY (tournament_id, player_id)
);

DROP TABLE pb_user;
CREATE TABLE pb_user(
	user_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	token VARCHAR(50) NULL,
	user_name VARCHAR(50) NOT NULL,
	password VARCHAR(50) NULL,
	salt VARCHAR(50) NULL,
	role_id INT NOT NULL,
	real_name VARCHAR(50) NULL,
	display_name VARCHAR(50) NOT NULL,
	email VARCHAR(50) NULL
);

DROP TABLE user_sharing;
CREATE TABLE user_sharing(
	user_id INT NOT NULL,
	service_name VARCHAR(50) NOT NULL,
    PRIMARY KEY (user_id, service_name)
);

DROP TABLE user_twitter;
CREATE TABLE user_twitter(
	user_id INT NOT NULL PRIMARY KEY,
	twitter_name VARCHAR(100) NOT NULL,
	key VARCHAR(100) NOT NULL,
	secret VARCHAR(100) NOT NULL
);

DROP TABLE video;
CREATE TABLE video(
	video_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	homegame_id INT NOT NULL,
	date date NOT NULL,
	thumbnail VARCHAR(255) NOT NULL,
	length INT NOT NULL,
	width INT NOT NULL,
	height INT NOT NULL,
	source VARCHAR(20) NOT NULL,
	type VARCHAR(20) NOT NULL,
	hidden bit NOT NULL
);

------------------------------------- ^ done (well kinda) ^

ALTER TABLE game ADD CONSTRAINT df_game_timestamp DEFAULT (getdate()) FOR timestamp;

ALTER TABLE game ADD DEFAULT ((0)) FOR homegame_id;

ALTER TABLE game ADD DEFAULT ((0)) FOR location_id;

ALTER TABLE location ADD DEFAULT ((0)) FOR bunch_id;

ALTER TABLE player ADD DEFAULT ((0)) FOR approved;

ALTER TABLE cashgame_checkpoint WITH CHECK ADD CONSTRAINT fk_cashgame_checkpoint_game FOREIGN KEY(game_id)
REFERENCES game (game_id);

ALTER TABLE cashgame_checkpoint CHECK CONSTRAINT fk_cashgame_checkpoint_game;

ALTER TABLE cashgame_checkpoint WITH CHECK ADD CONSTRAINT fk_cashgame_checkpoint_player FOREIGN KEY(player_id)
REFERENCES player (player_id);

ALTER TABLE cashgame_checkpoint CHECK CONSTRAINT fk_cashgame_checkpoint_player;
