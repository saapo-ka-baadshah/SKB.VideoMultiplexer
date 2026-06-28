#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(dirname "$0")"
KCLOGIN_DIR="$(realpath "${SCRIPT_DIR}/../SKB.VideoMultiplexer.KCLogin")"

echo "[1/2] Building Keycloak login theme from SKB.VideoMultiplexer.KCLogin ..."
npm --prefix "${KCLOGIN_DIR}" run build-keycloak-theme

echo "[2/2] Starting services with docker compose ..."
docker compose -f "${SCRIPT_DIR}/docker-compose.yml" --env-file "${SCRIPT_DIR}/.env" up -d

echo "Done."
