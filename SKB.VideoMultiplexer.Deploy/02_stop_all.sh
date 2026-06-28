#!/usr/bin/env bash

SCRIPT_DIR="$(dirname "$0")"

docker compose -f "${SCRIPT_DIR}/docker-compose.yml" --env-file "${SCRIPT_DIR}/.env" down
