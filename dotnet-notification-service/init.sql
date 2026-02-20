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
    IF NOT EXISTS(SELECT 1 FROM users."__EFMigrationsHistory" WHERE "MigrationId" = '20260203055934_InitialUserCreate') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'users') THEN
            CREATE SCHEMA users;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM users."__EFMigrationsHistory" WHERE "MigrationId" = '20260203055934_InitialUserCreate') THEN
    CREATE TABLE users.users (
        "Id" text NOT NULL,
        "Email" character varying(255) NOT NULL,
        "PasswordHash" text NOT NULL,
        CONSTRAINT "PK_users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM users."__EFMigrationsHistory" WHERE "MigrationId" = '20260203055934_InitialUserCreate') THEN
    CREATE UNIQUE INDEX "IX_users_Email" ON users.users ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM users."__EFMigrationsHistory" WHERE "MigrationId" = '20260203055934_InitialUserCreate') THEN
    INSERT INTO users."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260203055934_InitialUserCreate', '10.0.2');
    END IF;
END $EF$;
COMMIT;

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
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260203210706_InitialNotificationCreate') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'notifications') THEN
            CREATE SCHEMA notifications;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260203210706_InitialNotificationCreate') THEN
    CREATE TABLE notifications."Notifications" (
        "NotificationId" text NOT NULL,
        "Title" text NOT NULL,
        "Content" text NOT NULL,
        "Recipient" text NOT NULL,
        "CreatedBy" text NOT NULL,
        "Channel" text NOT NULL,
        CONSTRAINT "PK_Notifications" PRIMARY KEY ("NotificationId")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260203210706_InitialNotificationCreate') THEN
    INSERT INTO notifications."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260203210706_InitialNotificationCreate', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260220005729_AddTimestampsToNotification') THEN
    ALTER TABLE notifications."Notifications" ADD "SentAt" timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260220005729_AddTimestampsToNotification') THEN
    INSERT INTO notifications."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260220005729_AddTimestampsToNotification', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260220011947_AddSendMetaDataToNotification') THEN
    ALTER TABLE notifications."Notifications" ADD "SendMetadata" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260220011947_AddSendMetaDataToNotification') THEN
    INSERT INTO notifications."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260220011947_AddSendMetaDataToNotification', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM notifications."__EFMigrationsHistory" WHERE "MigrationId" = '20260220214822_FixSendMetadataColumn') THEN
    INSERT INTO notifications."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260220214822_FixSendMetadataColumn', '10.0.2');
    END IF;
END $EF$;
COMMIT;


