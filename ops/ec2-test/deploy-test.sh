#!/usr/bin/env bash
set -euo pipefail

REPO_DIR=${1:-$HOME/FractalGenerator}

cd "$REPO_DIR"
cp -n .env.test.example .env.test || true

docker compose -f docker-compose.test.yml --env-file .env.test up --build --abort-on-container-exit
