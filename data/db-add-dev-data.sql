INSERT INTO
    pb_user (user_name, display_name, password, salt, role_id)
VALUES
    (
        'admin',
        'Admin',
        '425af12a0743502b322e93a015bcf868e324d56a',
        'efgh',
        3
    );

INSERT INTO
    pb_user (user_name, display_name, password, salt, role_id)
VALUES
    (
        'manager',
        'Manager',
        '425af12a0743502b322e93a015bcf868e324d56a',
        'efgh',
        2
    );

INSERT INTO
    pb_user (user_name, display_name, password, salt, role_id)
VALUES
    (
        'player',
        'Player',
        '425af12a0743502b322e93a015bcf868e324d56a',
        'efgh',
        1
    );

INSERT INTO
    pb_bunch (
        name,
        display_name,
        description,
        timezone,
        default_buyin,
        currency,
        currency_layout,
        cashgames_enabled,
        tournaments_enabled,
        videos_enabled,
        house_rules
    )
VALUES
    (
        'test-bunch',
        'Test Bunch',
        '',
        'W. Europe Standard Time',
        200,
        'kr',
        '{AMOUNT} {SYMBOL}',
        true,
        false,
        false,
        ''
    );

INSERT INTO
    pb_player (bunch_id, user_id, role_id, approved)
VALUES
    (
        1,
        2,
        2,
        true
    );

INSERT INTO
    pb_player (bunch_id, user_id, role_id, approved)
VALUES
    (
        1,
        3,
        1,
        true
    );

INSERT INTO
    pb_location (name, bunch_id)
VALUES
    ('Test Location', 1);