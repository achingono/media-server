#!/bin/bash
createdbyid='c2214872-ba70-4aef-9346-f5e365dc1966'
createdbyemail='system@localhost'
createdbyfullname='System'

function generate_guid() {
  cat /proc/sys/kernel/random/uuid
}

function insert_artist() {
  local artist="$1"
  local artist_id

  # Escape single quotes
  artist=$(echo "$artist" | sed "s/'/''/g")

  # Check if artist already exists and insert if not
  artist_id=$(sqlite3 "$database_path" "SELECT Id FROM Artists WHERE Name = '$artist' COLLATE NOCASE")
  if [ -z "$artist_id" ]; then
    artist_id=$(sqlite3 "$database_path" "INSERT INTO Artists (Id, Name, Description, CreatedOn, CreatedBy_Id, CreatedBy_Email, CreatedBy_FullName)
      VALUES ('$(generate_guid)', '$artist', '', datetime('now'), '$createdbyid', '$createdbyemail', '$createdbyfullname')
      RETURNING Id")
  fi

  # Return artist id
  echo "$artist_id"
}

function insert_album() {
  local artist_id="$1"
  local album="$2"
  local album_id

  # Escape single quotes
  artist=$(echo "$artist" | sed "s/'/''/g")
  album=$(echo "$album" | sed "s/'/''/g")

  # Create album if one does not already exists
  album_id=$(sqlite3 "$database_path" "SELECT Id FROM Albums WHERE Name = '$album' AND ArtistId = '$artist_id'")
  if [ -z "$album_id" ]; then
    album_id=$(sqlite3 "$database_path" "INSERT INTO Albums (
      Id, Name, Description, ArtistId, GenreId, ReleasedOn, CreatedOn, CreatedBy_Id, CreatedBy_Email, CreatedBy_FullName
      )
      VALUES (
        '$(generate_guid)', '$album', '', '$artist_id', 'ed71e308-14e7-4464-8f23-37aeae4a0703', 
        datetime('now'), datetime('now'), '$createdbyid', '$createdbyemail', '$createdbyfullname'
      )
      RETURNING Id")
  fi

  # return album id from the database
  echo "$album_id"
}

function insert_track() {
  local album_id="$1"
  local track_number="$2"
  local track_title="$3"
  local track_duration="$4"
  local track_path="$5"
  local track_id=$(generate_guid)

  # Escape single quotes
  artist=$(echo "$artist" | sed "s/'/''/g")
  album=$(echo "$album" | sed "s/'/''/g")
  track_title=$(echo "$track_title" | sed "s/'/''/g")
  track_path=$(echo "$track_path" | sed "s/'/''/g")

  # Insert track
  sqlite3 "$database_path" "INSERT INTO Tracks (
      Id, Name, Number, Duration, Path, AlbumId, CreatedOn, CreatedBy_Id, CreatedBy_Email, CreatedBy_FullName
    ) 
    VALUES (
      '$track_id', '$track_title', $track_number, $track_duration, '$track_path', 
      '$album_id', datetime('now'), '$createdbyid', '$createdbyemail', '$createdbyfullname'
    )"

  # return track id
  echo "$track_id"
}

function insert_image() {
  local album_id="$1"
  local image_path="$2"
  local image_id=$(generate_guid)

  # Escape single quotes  
  image_path=$(echo "$image_path" | sed "s/'/''/g")

  # Insert image
  sqlite3 "$database_path" "INSERT INTO Images (
      Id, Size, Url, Discriminator, AlbumId, CreatedOn, CreatedBy_Id, CreatedBy_Email, CreatedBy_FullName
    ) 
    SELECT 
      '$image_id', 2, '$image_path', 'AlbumImage', '$album_id', 
      datetime('now'), '$createdbyid', '$createdbyemail', '$createdbyfullname'
    WHERE NOT EXISTS (
      SELECT 1 FROM Images WHERE Url = '$image_path'
    )"
}

# Main script logic
input_dir="$1"
database_path="$2"

if [ -z "$input_dir" ]; then
  echo "Please provide an input directory as an argument."
  exit 1
fi

if [ -z "$database_path" ]; then
  echo "Please provide a database path as an argument."
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

  # Extract metadata from JSON output
  track_artist=$(jq -r '.streams[0].tags.ARTIST // .format.tags.ARTIST' <<< "$ffprobe_output")
  album_name=$(jq -r '.streams[0].tags.ALBUM // .format.tags.ALBUM' <<< "$ffprobe_output")
  album_artist=$(jq -r '.streams[0].tags.album_artist // .format.tags.album_artist' <<< "$ffprobe_output")
  track_number=$(jq -r '.streams[0].tags.track // .format.tags.track' <<< "$ffprobe_output")
  track_title=$(jq -r '.streams[0].tags.TITLE // .format.tags.TITLE' <<< "$ffprobe_output")
  track_genre=$(jq -r '.streams[0].tags.GENRE // .format.tags.GENRE' <<< "$ffprobe_output")
  album_collaborators=()
  track_collaborators=()

  # Use metadata if available, otherwise use values from file name
  # Fallback to file path if metadata is empty or missing
  if [[ -z "$album_artist" ]]; then
    album_artist=$(basename "$artist")
  fi

  if [[ -z "$track_artist" ]]; then
    track_artist=$(basename "$artist")
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

  # Handle potential collaboration indicators
  if [[ "$album_artist" =~ "," ]] || [[ "$album_artist" =~ "feat." ]] || [[ "$album_artist" =~ "featuring" ]] || [[ "$album_artist" =~ "with" ]] || [[ "$album_artist" =~ "&" ]]; then
    # Extract the primary artist (before any collaborator indicator)
    primary_artist=$(echo "$album_artist" | 
      sed 's/ feat\.\(.*\)/\1/i;s/ featuring\(.*\)/\1/i;s/ with\(.*\)/\1/i;s/ \&\ \(.*\)/\1/i' | 
      cut -d',' -f1) 

    # Split artist names at all collaboration indicators
    IFS=',' read -ra album_collaborators <<< "$(echo "$album_artist" | sed 's/ feat\.\(.*\)/,\1/i;s/ featuring\(.*\)/,\1/i;s/ with\(.*\)/,\1/i;s/ \&\ \(.*\)/,\1/i')"
  else
    primary_artist="$album_artist" 
  fi

  # Handle potential collaboration indicators
  if [[ "$track_artist" =~ "," ]] || [[ "$track_artist" =~ "feat." ]] || [[ "$track_artist" =~ "featuring" ]] || [[ "$track_artist" =~ "with" ]] || [[ "$track_artist" =~ "&" ]]; then
    # Split artist names at all collaboration indicators
    IFS=',' read -ra track_collaborators <<< "$(echo "$track_artist" | sed 's/ feat\.\(.*\)/,\1/i;s/ featuring\(.*\)/,\1/i;s/ with\(.*\)/,\1/i;s/ \&\ \(.*\)/,\1/i')"
  fi

  # Insert the primary artist
  artist_id=$(insert_artist "$primary_artist")

  # Insert the album
  echo "Inserting album: $album_name"
  album_id=$(insert_album "$artist_id" "$album_name")

  # Insert collaborating artists (if needed)
  if [ ${#album_collaborators[@]} -gt 0 ]; then
    declare -p album_collaborators
    for collaborator in "${album_collaborators[@]}"; do
      # Remove potential collaboration indicators from each artist
      collaborator=$(echo "$collaborator" | sed 's/feat.//;s/featuring//;s/with//;s/\&//') 
      echo "Inserting album collaborator: $collaborator"
      collaborator_id=$(insert_artist "$collaborator")

      # insert album collaborator
      echo "Inserting album collaborator: $collaborator"
      sqlite3 "$database_path" "INSERT INTO AlbumFeature (
          AlbumId, ArtistId
        )
        SELECT '$album_id', '$collaborator_id'
        WHERE NOT EXISTS (
          SELECT 1 FROM AlbumFeature WHERE AlbumId = '$album_id' AND ArtistId = '$collaborator_id'
        )"
    done
  fi

  echo "Inserting track: $track_number $track_title | $track_path"
  track_id=$(insert_track "$album_id" "$track_number" "$track_title" "$track_duration" "$track_path")

  # Insert collaborating artists (if needed)
  if [ ${#track_collaborators[@]} -gt 0 ]; then
    declare -p track_collaborators
    for collaborator in "${track_collaborators[@]}"; do
      # Remove potential collaboration indicators from each artist
      collaborator=$(echo "$collaborator" | sed 's/feat.//;s/featuring//;s/with//;s/\&//') 
      echo "Inserting track collaborator: $collaborator"
      collaborator_id=$(insert_artist "$collaborator")

      # insert track collaborator
      echo "Inserting track collaborator: $collaborator"
      sqlite3 "$database_path" "INSERT INTO TrackFeature (
        TrackId, ArtistId
        )
        SELECT '$track_id', '$collaborator_id'
        WHERE NOT EXISTS (
          SELECT 1 FROM TrackFeature WHERE TrackId = '$track_id' AND ArtistId = '$collaborator_id'
        )"
    done
  fi

  album_cover="$album/cover.jpg"
  # check if 'cover.jpg' or 'AlbumArt.jpg' exists
  if [ -f "$album_cover" ]; then
    # cover image found
    cover_image_path=$(realpath "$album_cover" | sed "s|^$input_dir||")
    
    # remove leading slash (/) if it exists
    cover_image_path="${cover_image_path#\/}"

    # insert cover
    echo "Inserting cover image: '$cover_image_path'"
    insert_image "$album_id" "$cover_image_path"
  fi

  album_art="$album/AlbumArt.jpg"
  if [ -f "$album_art" ]; then
    # album art image found
    cover_image_path=$(realpath "$album_art" | sed "s|^$input_dir||")
    
    # remove leading slash (/) if it exists
    cover_image_path="${cover_image_path#\/}"

    # insert cover
    echo "Inserting cover image: '$cover_image_path'"
    insert_image "$album_id" "$cover_image_path"
  fi
done
