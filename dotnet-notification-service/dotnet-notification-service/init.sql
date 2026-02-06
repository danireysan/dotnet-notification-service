DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'users') THEN
        CREATE SCHEMA users;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS users."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'users') THEN
        CREATE SCHEMA users;
    END IF;
END $EF$;

CREATE TABLE users.users (
    "Id" text NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    CONSTRAINT "PK_users" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_users_Email" ON users.users ("Email");

INSERT INTO users."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260203055934_InitialUserCreate', '10.0.2');

COMMIT;

Build started...
Build succeeded.
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notifications') THEN
        CREATE SCHEMA notifications;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS notifications."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notifications') THEN
        CREATE SCHEMA notifications;
    END IF;
END $EF$;

CREATE TABLE notifications."Notifications" (
    "NotificationId" text NOT NULL,
    "Title" text NOT NULL,
    "Content" text NOT NULL,
    "Recipient" text NOT NULL,
    "CreatedBy" text NOT NULL,
    "Channel" text NOT NULL,
    CONSTRAINT "PK_Notifications" PRIMARY KEY ("NotificationId")
);

INSERT INTO notifications."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260203210706_InitialNotificationCreate', '10.0.2');

COMMIT;


