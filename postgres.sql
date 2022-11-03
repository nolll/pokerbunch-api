DROP TABLE IF EXISTS pb_app;
DROP TABLE IF EXISTS pb_cashgame_checkpoint;
DROP TABLE IF EXISTS pb_cashgame_comment;
DROP TABLE IF EXISTS pb_event_cashgame;
DROP TABLE IF EXISTS pb_game;
DROP TABLE IF EXISTS pb_location;
DROP TABLE IF EXISTS pb_tournament_comment;
DROP TABLE IF EXISTS pb_tournament_payout;
DROP TABLE IF EXISTS pb_tournament_result;
DROP TABLE IF EXISTS pb_user_sharing;
DROP TABLE IF EXISTS pb_user_twitter;
DROP TABLE IF EXISTS pb_video;
DROP TABLE IF EXISTS pb_event;
DROP TABLE IF EXISTS pb_tournament;
DROP TABLE IF EXISTS pb_comment;
DROP TABLE IF EXISTS pb_player;
DROP TABLE IF EXISTS pb_bunch;
DROP TABLE IF EXISTS pb_user;
DROP TABLE IF EXISTS pb_role;

CREATE TABLE pb_bunch(
	bunch_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	display_name VARCHAR(50) NULL,
	description VARCHAR(50) NULL,
	timezone VARCHAR(50) NOT NULL,
	default_buyin INT NOT NULL,
	currency VARCHAR(3) NOT NULL,
	currency_layout VARCHAR(20) NOT NULL,
	cashgames_enabled BOOLEAN NOT NULL,
	tournaments_enabled BOOLEAN NOT NULL,
	videos_enabled BOOLEAN NOT NULL,
	house_rules TEXT NULL
);

CREATE TABLE pb_event(
	event_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	bunch_id INT DEFAULT 0 NOT NULL,
    CONSTRAINT fk_bunch
        FOREIGN KEY(bunch_id)
        REFERENCES pb_bunch(bunch_id)
);

CREATE TABLE pb_location(
	location_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	bunch_id INT DEFAULT 0 NOT NULL,
    CONSTRAINT fk_bunch
        FOREIGN KEY(bunch_id)
        REFERENCES pb_bunch(bunch_id)
);

CREATE TABLE pb_game(
	game_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	date date NOT NULL,
	timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
	status INT NOT NULL,
	bunch_id INT DEFAULT 0 NOT NULL,
	location_id INT DEFAULT 0 NOT NULL,
    CONSTRAINT fk_bunch
        FOREIGN KEY(bunch_id)
        REFERENCES pb_bunch(bunch_id),
    CONSTRAINT fk_location
        FOREIGN KEY(location_id)
        REFERENCES pb_location(location_id)
);

CREATE TABLE pb_event_cashgame(
	event_id INT NOT NULL,
	game_id INT NOT NULL,
    PRIMARY KEY (event_id, game_id),
    CONSTRAINT fk_event
        FOREIGN KEY(event_id)
        REFERENCES pb_event(event_id),
    CONSTRAINT fk_game
        FOREIGN KEY(game_id)
        REFERENCES pb_game(game_id)
);

CREATE TABLE pb_player(
	player_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	bunch_id INT NOT NULL,
	user_id INT NULL,
	role_id INT NOT NULL,
	approved BOOLEAN DEFAULT FALSE NOT NULL,
	player_name VARCHAR(50) NULL,
	color VARCHAR(10) NULL,
    CONSTRAINT fk_bunch
        FOREIGN KEY(bunch_id)
        REFERENCES pb_bunch(bunch_id)
);

CREATE TABLE pb_cashgame_checkpoint(
	checkpoint_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	game_id INT NOT NULL,
	player_id INT NOT NULL,
	type INT DEFAULT 0 NOT NULL,
	amount INT DEFAULT 0 NOT NULL,
	stack INT DEFAULT 0 NOT NULL,
	timestamp TIMESTAMP NOT NULL,
    CONSTRAINT fk_game
        FOREIGN KEY(game_id)
        REFERENCES pb_game(game_id),
    CONSTRAINT fk_player
        FOREIGN KEY(player_id)
        REFERENCES pb_player(player_id)
);

CREATE TABLE pb_comment(
	comment_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	player_id INT NOT NULL,
	date TIMESTAMP NOT NULL,
	comment_text VARCHAR(1024) NOT NULL,
    CONSTRAINT fk_player
        FOREIGN KEY(player_id)
        REFERENCES pb_player(player_id)
);

CREATE TABLE pb_cashgame_comment(
	game_id INT NOT NULL,
	comment_id INT NOT NULL,
    PRIMARY KEY (game_id, comment_id),
    CONSTRAINT fk_game
        FOREIGN KEY(game_id)
        REFERENCES pb_game(game_id),
    CONSTRAINT fk_comment
        FOREIGN KEY(comment_id)
        REFERENCES pb_comment(comment_id)
);

CREATE TABLE pb_role(
	role_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	role_name VARCHAR(50) NOT NULL
);

CREATE TABLE pb_tournament(
	tournament_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	bunch_id INT NOT NULL,
	buyin INT NOT NULL,
	date date NOT NULL,
	duration INT NOT NULL,
	location VARCHAR(50) NOT NULL,
	timestamp TIMESTAMP NOT NULL,
	published BOOLEAN NOT NULL,
    CONSTRAINT fk_bunch
        FOREIGN KEY(bunch_id)
        REFERENCES pb_bunch(bunch_id)
);

CREATE TABLE pb_tournament_comment(
	tournament_id INT NOT NULL,
	comment_id INT NOT NULL,
    PRIMARY KEY (tournament_id, comment_id),
    CONSTRAINT fk_tournament
        FOREIGN KEY(tournament_id)
        REFERENCES pb_tournament(tournament_id),
    CONSTRAINT fk_comment
        FOREIGN KEY(comment_id)
        REFERENCES pb_comment(comment_id)
);

CREATE TABLE pb_tournament_payout(
	tournament_id INT NOT NULL,
	position INT NOT NULL,
	payout INT NOT NULL,
    PRIMARY KEY (tournament_id, position),
    CONSTRAINT fk_tournament
        FOREIGN KEY(tournament_id)
        REFERENCES pb_tournament(tournament_id)
);

CREATE TABLE pb_tournament_result(
	tournament_id INT NOT NULL,
	player_id INT NOT NULL,
	position INT NOT NULL,
    PRIMARY KEY (tournament_id, player_id),
    CONSTRAINT fk_tournament
        FOREIGN KEY(tournament_id)
        REFERENCES pb_tournament(tournament_id),
    CONSTRAINT fk_player
        FOREIGN KEY(player_id)
        REFERENCES pb_player(player_id)
);

CREATE TABLE pb_user(
	user_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	token VARCHAR(50) NULL,
	user_name VARCHAR(50) NOT NULL,
	password VARCHAR(50) NULL,
	salt VARCHAR(50) NULL,
	role_id INT NOT NULL,
	real_name VARCHAR(50) NULL,
	display_name VARCHAR(50) NOT NULL,
	email VARCHAR(50) NULL,
    CONSTRAINT fk_role
        FOREIGN KEY(role_id)
        REFERENCES pb_role(role_id)
);

CREATE TABLE pb_app(
	app_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	app_key VARCHAR(50) NOT NULL,
	name VARCHAR(50) NOT NULL,
	user_id INT NOT NULL,
    CONSTRAINT fk_user
        FOREIGN KEY(user_id)
        REFERENCES pb_user(user_id)
);

CREATE TABLE pb_user_sharing(
	user_id INT NOT NULL,
	service_name VARCHAR(50) NOT NULL,
    PRIMARY KEY (user_id, service_name),
    CONSTRAINT fk_user
        FOREIGN KEY(user_id)
        REFERENCES pb_user(user_id)
);

CREATE TABLE pb_user_twitter(
	user_id INT NOT NULL PRIMARY KEY,
	twitter_name VARCHAR(100) NOT NULL,
	key VARCHAR(100) NOT NULL,
	secret VARCHAR(100) NOT NULL,
    CONSTRAINT fk_user
        FOREIGN KEY(user_id)
        REFERENCES pb_user(user_id)
);

CREATE TABLE pb_video(
	video_id INT GENERATED BY DEFAULT AS IDENTITY NOT NULL PRIMARY KEY,
	bunch_id INT NOT NULL,
	date date NOT NULL,
	thumbnail VARCHAR(255) NOT NULL,
	length INT NOT NULL,
	width INT NOT NULL,
	height INT NOT NULL,
	source VARCHAR(20) NOT NULL,
	type VARCHAR(20) NOT NULL,
	hidden BOOLEAN NOT NULL,
    CONSTRAINT fk_bunch
        FOREIGN KEY(bunch_id) 
        REFERENCES pb_bunch(bunch_id)
);

INSERT INTO pb_role (role_id, role_name) VALUES (1, 'Player');
INSERT INTO pb_role (role_id, role_name) VALUES (2, 'Manager');
INSERT INTO pb_role (role_id, role_name) VALUES (3, 'Admin');
