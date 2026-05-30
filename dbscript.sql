-- 1/

CREATE TABLE pilot_user (
    iduser      SERIAL PRIMARY KEY,
    username    VARCHAR(50) NOT NULL UNIQUE,
    password    VARCHAR(255) NOT NULL,
    firstname   VARCHAR(100) NOT NULL,
    email       VARCHAR(255) NOT NULL
);

-- 2/

CREATE TABLE pilot_exam (
    idexam         SERIAL PRIMARY KEY,
    iduser         INTEGER NOT NULL,
    score          REAL NOT NULL,
    mode           VARCHAR(10) NOT NULL,
    title          VARCHAR(10) NOT NULL,
    nb_questions   INTEGER NOT NULL,
    duration       VARCHAR(10) NOT NULL,
    exam_date      VARCHAR(10) NOT NULL
);

-- 3/ 

ALTER TABLE pilot_exam 
ADD COLUMN answer_string TEXT NOT NULL DEFAULT '';

-- 4/ 

ALTER TABLE pilot_user
ADD COLUMN role TEXT NOT NULL DEFAULT 'User';