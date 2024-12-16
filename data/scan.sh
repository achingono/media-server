#!/bin/bash
createdbyid='c2214872-ba70-4aef-9346-f5e365dc1966'

function generate_guid() {
  cat /proc/sys/kernel/random/uuid
}

function insert_artist() {
  local artist="$1"
  local artist_id=$(generate_guid)

  # Check if artist already exists
  sqlite3 Library.db "INSERT INTO Artists (Id, Name, Description, CreatedOn, CreatedById)
    SELECT '$artist_id', '$artist', '', datetime('now'), '$createdbyid'
    WHERE NOT EXISTS (SELECT 1 FROM Artists WHERE Name = '$artist')"
}

function insert_album() {
  local artist="$1"
  local album="$2"
  local album_id=$(generate_guid)

  sqlite3 Library.db "INSERT INTO Albums (Id, Name, Description, ArtistId, GenreId, ReleasedOn, CreatedOn, CreatedById)
    SELECT '$album_id', '$album', '', (SELECT Id FROM Artists WHERE Name = '$artist'), 'ed71e308-14e7-4464-8f23-37aeae4a0703', datetime('now'), datetime('now'), '$createdbyid'
    WHERE NOT EXISTS (SELECT 1 FROM Albums WHERE Name = '$album' AND ArtistId = (SELECT Id FROM Artists WHERE Name = '$artist'))"
}

function insert_track() {
  local artist="$1"
  local album="$2"
  local track_number="$3"
  local track_title="$4"
  local track_duration="$5"
  local track_path="$6"
  local track_id=$(generate_guid)

  # Insert track
  sqlite3 Library.db "INSERT INTO Tracks (Id, Name, Number, Duration, Path, AlbumId, CreatedOn, CreatedById) 
    VALUES ('$track_id', '$track_title', $track_number, $track_duration, '$track_path', (SELECT Id FROM Albums WHERE Name = '$album' AND ArtistId = (SELECT Id FROM Artists WHERE Name = '$artist')), datetime('now'), '$createdbyid')"
}

# Main script logic
input_dir="$1"

if [ -z "$input_dir" ]; then
  echo "Please provide an input directory as an argument."
  exit 1
fi

echo "Scanning directory: '$input_dir'"

find "$input_dir" -type f \( -name "*.mp3" -o -name "*.flac" -o -name "*.wav" -o -name "*.ogg" \) -print0 | while IFS= read -r -d '' file; do
  artist=$(dirname "$(dirname "$file")")
  album=$(dirname "$file")
  number=$(basename "$file" | cut -d' ' -f1)
  title=$(basename "$file" | cut -d' ' -f2-)
  track_path=$(realpath "$file" | sed "s|^$input_dir||")
  # remove leading slash (/) if it exists
  track_path="${track_path#\/}"
  # Get duration using ffprobe
  track_duration=$(ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 "$file")

  # Extract metadata using ffprobe
  ffprobe_output=$(ffprobe -v error -show_entries stream_tags:format_tags -of json "$file")

  # Parse JSON output
  artist_name=$(jq -r '.streams[0].tags.ARTIST' <<< "$ffprobe_output")
  album_name=$(jq -r '.streams[0].tags.ALBUM' <<< "$ffprobe_output")
  track_number=$(jq -r '.streams[0].tags.track' <<< "$ffprobe_output")
  track_title=$(jq -r '.streams[0].tags.TITLE' <<< "$ffprobe_output")
  track_genre=$(jq -r '.streams[0].tags.GENRE' <<< "$ffprobe_output")

  # Use metadata if available, otherwise use values from file name
  # Fallback to file path if metadata is empty or missing
  if [[ -z "$artist_name" ]]; then
    artist_name=$(basename "$artist")
  fi

  if [[ -z "$album_name" ]]; then
    album_name=$(basename "$album")
  fi
  if [[ -n "$track_number" ]]; then
    track_number="$number"
  fi
  if [[ -n "$track_title" ]]; then
    track_title="${title%.*}"
  fi

  echo "Inserting artist: $artist_name"
  insert_artist "$artist_name"

  echo "Inserting album: $album_name"
  insert_album "$artist_name" "$album_name"

  echo "Inserting track: $track_number $track_title | $track_path"
  insert_track "$artist_name" "$album_name" "$track_number" "$track_title" "$track_duration" "$track_path"
done