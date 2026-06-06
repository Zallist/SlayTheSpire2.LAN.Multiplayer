#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT_PATH="$ROOT_DIR/SlayTheSpire2.LAN.Multiplayer/SlayTheSpire2.LAN.Multiplayer.csproj"
PROJECT_DIR="$ROOT_DIR/SlayTheSpire2.LAN.Multiplayer"
MOD_ID="SlayTheSpire2.LAN.Multiplayer"
BUILD_CONFIGURATION="${BUILD_CONFIGURATION:-Release}"
TARGET_FRAMEWORK="net9.0"
OUTPUT_DIR="${OUTPUT_DIR:-$ROOT_DIR/artifacts}"
PACKAGE_ROOT="$OUTPUT_DIR/mods/$MOD_ID"

if ! command -v dotnet >/dev/null 2>&1; then
  echo "error: dotnet SDK was not found. Install .NET 9 SDK first."
  exit 1
fi

if [[ -z "${STS2_INSTALL_DIR:-}" && -z "${STS2_DATA_DIR:-}" ]]; then
  STS2_INSTALL_DIR="$HOME/Library/Application Support/Steam/steamapps/common/Slay the Spire 2/SlayTheSpire2.app/Contents/Resources"
fi

BUILD_ARGS=(
  build
  "$PROJECT_PATH"
  -c "$BUILD_CONFIGURATION"
  /p:ContinuousIntegrationBuild=true
)

if [[ -n "${STS2_DATA_DIR:-}" ]]; then
  BUILD_ARGS+=("/p:Sts2DataDir=$STS2_DATA_DIR")
fi
if [[ -n "${STS2_INSTALL_DIR:-}" ]]; then
  BUILD_ARGS+=("/p:Sts2InstallDir=$STS2_INSTALL_DIR")
fi

dotnet "${BUILD_ARGS[@]}"

DLL_PATH="$PROJECT_DIR/bin/$BUILD_CONFIGURATION/$TARGET_FRAMEWORK/$MOD_ID.dll"
if [[ ! -f "$DLL_PATH" ]]; then
  echo "error: build succeeded but output DLL was not found at '$DLL_PATH'"
  exit 1
fi

rm -rf "$PACKAGE_ROOT"
mkdir -p "$PACKAGE_ROOT"
cp "$DLL_PATH" "$PACKAGE_ROOT/$MOD_ID.dll"

if [[ -f "$ROOT_DIR/mod_image.png" ]]; then
  cp "$ROOT_DIR/mod_image.png" "$PACKAGE_ROOT/mod_image.png"
fi

ASSEMBLY_VERSION="$(sed -n 's:.*<AssemblyVersion>\(.*\)</AssemblyVersion>.*:\1:p' "$PROJECT_PATH" | head -n 1)"
MOD_VERSION="${MOD_VERSION:-${ASSEMBLY_VERSION%.0}}"

cat > "$PACKAGE_ROOT/mod_manifest.json" <<EOF
{
  "id": "$MOD_ID",
  "name": "$MOD_ID",
  "author": "Kmyuhkyuk",
  "description": "Added Local network multiplayer feature interface, Custom host port and maximum players settings.",
  "version": "$MOD_VERSION",
  "has_pck": false,
  "has_dll": true,
  "dependencies": [],
  "affects_gameplay": true
}
EOF

ARCHIVE_NAME="$MOD_ID.Release_$MOD_VERSION"
if command -v 7zz >/dev/null 2>&1; then
  (
    cd "$OUTPUT_DIR"
    rm -f "$ARCHIVE_NAME.7z"
    7zz a -t7z "$ARCHIVE_NAME.7z" mods >/dev/null
  )
  echo "Created: $OUTPUT_DIR/$ARCHIVE_NAME.7z"
else
  (
    cd "$OUTPUT_DIR"
    rm -f "$ARCHIVE_NAME.zip"
    zip -qr "$ARCHIVE_NAME.zip" mods
  )
  echo "Created: $OUTPUT_DIR/$ARCHIVE_NAME.zip"
fi

echo "Package root: $PACKAGE_ROOT"
