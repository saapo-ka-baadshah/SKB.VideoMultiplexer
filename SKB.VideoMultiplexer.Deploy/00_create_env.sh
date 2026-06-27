#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(dirname "$0")"
ENV_FILE="${SCRIPT_DIR}/.env"
REALM_FILE="${SCRIPT_DIR}/config/keycloak/skb-videomultiplexer-realm.json"

if [ -f "$ENV_FILE" ]; then
  read -rp ".env already exists. Overwrite? [y/N] " yn
  case "$yn" in
    [yY]) ;;
    *) echo "Aborting."; exit 1 ;;
  esac
fi

KC_DB_PASSWORD="${KC_DB_PASSWORD:-$(openssl rand -base64 24)}"
KC_ADMIN_PASSWORD="${KC_ADMIN_PASSWORD:-$(openssl rand -base64 24)}"
KC_CLIENT_SECRET="${KC_CLIENT_SECRET:-$(openssl rand -base64 32)}"

cat > "$ENV_FILE" <<ENVEOF
# ─────────────────────────────────────────────────
# SKB.VideoMultiplexer — Environment Configuration
# ─────────────────────────────────────────────────

# Keycloak
KC_DB_PASSWORD=${KC_DB_PASSWORD}
KC_ADMIN_USER=${KC_ADMIN_USER:-admin}
KC_ADMIN_PASSWORD=${KC_ADMIN_PASSWORD}
KC_REALM=${KC_REALM:-skb-videomultiplexer}
KC_CLIENT_ID=${KC_CLIENT_ID:-skb-videomultiplexer}
KC_CLIENT_SECRET=${KC_CLIENT_SECRET}
KC_PORT=${KC_PORT:-8080}
KC_LOG_LEVEL=${KC_LOG_LEVEL:-INFO}

# Application
APP_PORT=${APP_PORT:-5000}
ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Development}
ENVEOF

echo "Created $ENV_FILE"

# ─────────────────────────────────────────────────
# Sync secrets into the Keycloak realm import file
# ─────────────────────────────────────────────────
if [ -f "$REALM_FILE" ]; then
  python3 - "$REALM_FILE" "$KC_ADMIN_PASSWORD" "$KC_CLIENT_SECRET" <<'PYEOF'
import json, sys

path, admin_pw, client_secret = sys.argv[1], sys.argv[2], sys.argv[3]

with open(path) as f:
    realm = json.load(f)

# Update admin user password
for user in realm.get("users", []):
    if user.get("username") == "admin":
        for cred in user.get("credentials", []):
            if cred.get("type") == "password":
                cred["value"] = admin_pw
                break

# Update client secret
client_id = realm.get("clients", [{}])[0].get("clientId", "skb-videomultiplexer")
for client in realm.get("clients", []):
    if client.get("clientId") == client_id or client.get("clientId") == "skb-videomultiplexer":
        client["secret"] = client_secret
        break

with open(path, "w") as f:
    json.dump(realm, f, indent=2)
    f.write("\n")

print(f"Synced secrets into {path}")
PYEOF
else
  echo "WARNING: $REALM_FILE not found — secrets not synced to realm config."
fi
