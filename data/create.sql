CREATE TABLE IF NOT EXISTS "Artists" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Artists" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "CreatedOn" TEXT NOT NULL,
    "CreatedBy_Id" TEXT NOT NULL,
    "CreatedBy_Email" TEXT NOT NULL,
    "CreatedBy_FullName" TEXT NOT NULL,
    "UpdatedOn" TEXT NULL,
    "UpdatedBy_Id" TEXT NULL,
    "UpdatedBy_Email" TEXT NULL,
    "UpdatedBy_FullName" TEXT NULL
);
CREATE TABLE IF NOT EXISTS "Genres" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Genres" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "CreatedOn" TEXT NOT NULL,
    "CreatedBy_Id" TEXT NOT NULL,
    "CreatedBy_Email" TEXT NOT NULL,
    "CreatedBy_FullName" TEXT NOT NULL,
    "UpdatedOn" TEXT NULL,
    "UpdatedBy_Id" TEXT NULL,
    "UpdatedBy_Email" TEXT NULL,
    "UpdatedBy_FullName" TEXT NULL
);
CREATE TABLE IF NOT EXISTS "Playlists" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Playlists" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "CreatedOn" TEXT NOT NULL,
    "CreatedBy_Id" TEXT NOT NULL,
    "CreatedBy_Email" TEXT NOT NULL,
    "CreatedBy_FullName" TEXT NOT NULL,
    "UpdatedOn" TEXT NULL,
    "UpdatedBy_Id" TEXT NULL,
    "UpdatedBy_Email" TEXT NULL,
    "UpdatedBy_FullName" TEXT NULL
);
CREATE TABLE IF NOT EXISTS "Albums" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Albums" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "ArtistId" TEXT NOT NULL,
    "GenreId" TEXT NOT NULL,
    "ReleasedOn" TEXT NOT NULL,
    "CreatedOn" TEXT NOT NULL,
    "CreatedBy_Id" TEXT NOT NULL,
    "CreatedBy_Email" TEXT NOT NULL,
    "CreatedBy_FullName" TEXT NOT NULL,
    "UpdatedOn" TEXT NULL,
    "UpdatedBy_Id" TEXT NULL,
    "UpdatedBy_Email" TEXT NULL,
    "UpdatedBy_FullName" TEXT NULL,
    CONSTRAINT "FK_Albums_Artists_ArtistId" FOREIGN KEY ("ArtistId") REFERENCES "Artists" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Albums_Genres_GenreId" FOREIGN KEY ("GenreId") REFERENCES "Genres" ("Id") ON DELETE RESTRICT
);
CREATE TABLE IF NOT EXISTS "AlbumFeature" (
    "AlbumId" TEXT NOT NULL,
    "ArtistId" TEXT NOT NULL,
    CONSTRAINT "PK_AlbumFeature" PRIMARY KEY ("AlbumId", "ArtistId"),
    CONSTRAINT "FK_AlbumFeature_Albums_AlbumId" FOREIGN KEY ("AlbumId") REFERENCES "Albums" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_AlbumFeature_Artists_ArtistId" FOREIGN KEY ("ArtistId") REFERENCES "Artists" ("Id") ON DELETE RESTRICT
);
CREATE TABLE IF NOT EXISTS "Images" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Images" PRIMARY KEY,
    "Size" INTEGER NOT NULL,
    "Url" TEXT NOT NULL,
    "CreatedOn" TEXT NOT NULL,
    "UpdatedOn" TEXT NULL,
    "Discriminator" TEXT NOT NULL,
    "AlbumId" TEXT NULL,
    "CreatedBy_Id" TEXT NULL,
    "CreatedBy_Email" TEXT NULL,
    "CreatedBy_FullName" TEXT NULL,
    "UpdatedBy_Id" TEXT NULL,
    "UpdatedBy_Email" TEXT NULL,
    "UpdatedBy_FullName" TEXT NULL,
    "ArtistId" TEXT NULL,
    "PlaylistId" TEXT NULL,
    CONSTRAINT "FK_Images_Albums_AlbumId" FOREIGN KEY ("AlbumId") REFERENCES "Albums" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Images_Artists_ArtistId" FOREIGN KEY ("ArtistId") REFERENCES "Artists" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Images_Playlists_PlaylistId" FOREIGN KEY ("PlaylistId") REFERENCES "Playlists" ("Id") ON DELETE RESTRICT
);
CREATE TABLE IF NOT EXISTS "Tracks" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Tracks" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Number" INTEGER NOT NULL,
    "Duration" INTEGER NOT NULL,
    "Path" TEXT NOT NULL,
    "CreatedOn" TEXT NOT NULL,
    "CreatedBy_Id" TEXT NOT NULL,
    "CreatedBy_Email" TEXT NOT NULL,
    "CreatedBy_FullName" TEXT NOT NULL,
    "UpdatedOn" TEXT NULL,
    "UpdatedBy_Id" TEXT NULL,
    "UpdatedBy_Email" TEXT NULL,
    "UpdatedBy_FullName" TEXT NULL,
    "AlbumId" TEXT NOT NULL,
    CONSTRAINT "FK_Tracks_Albums_AlbumId" FOREIGN KEY ("AlbumId") REFERENCES "Albums" ("Id") ON DELETE RESTRICT
);
CREATE TABLE IF NOT EXISTS "PlaylistTrack" (
    "PlaylistsId" TEXT NOT NULL,
    "TracksId" TEXT NOT NULL,
    CONSTRAINT "PK_PlaylistTrack" PRIMARY KEY ("PlaylistsId", "TracksId"),
    CONSTRAINT "FK_PlaylistTrack_Playlists_PlaylistsId" FOREIGN KEY ("PlaylistsId") REFERENCES "Playlists" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PlaylistTrack_Tracks_TracksId" FOREIGN KEY ("TracksId") REFERENCES "Tracks" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "TrackFeature" (
    "TrackId" TEXT NOT NULL,
    "ArtistId" TEXT NOT NULL,
    CONSTRAINT "PK_TrackFeature" PRIMARY KEY ("TrackId", "ArtistId"),
    CONSTRAINT "FK_TrackFeature_Artists_ArtistId" FOREIGN KEY ("ArtistId") REFERENCES "Artists" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_TrackFeature_Tracks_TrackId" FOREIGN KEY ("TrackId") REFERENCES "Tracks" ("Id") ON DELETE RESTRICT
);
CREATE INDEX "IX_AlbumFeature_ArtistId" ON "AlbumFeature" ("ArtistId");
CREATE INDEX "IX_Albums_ArtistId" ON "Albums" ("ArtistId");
CREATE INDEX "IX_Albums_GenreId" ON "Albums" ("GenreId");
CREATE UNIQUE INDEX "IX_Albums_Name_ArtistId" ON "Albums" ("Name", "ArtistId");
CREATE INDEX "IX_Images_AlbumId" ON "Images" ("AlbumId");
CREATE INDEX "IX_Images_ArtistId" ON "Images" ("ArtistId");
CREATE INDEX "IX_Images_PlaylistId" ON "Images" ("PlaylistId");
CREATE INDEX "IX_PlaylistTrack_TracksId" ON "PlaylistTrack" ("TracksId");
CREATE INDEX "IX_TrackFeature_ArtistId" ON "TrackFeature" ("ArtistId");
CREATE INDEX "IX_Tracks_AlbumId" ON "Tracks" ("AlbumId");