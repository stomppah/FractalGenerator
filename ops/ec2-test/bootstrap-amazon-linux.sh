#!/usr/bin/env bash
set -euo pipefail

sudo dnf update -y
sudo dnf install -y git docker
sudo systemctl enable --now docker
sudo usermod -aG docker ec2-user

if ! command -v docker-compose >/dev/null 2>&1; then
  sudo curl -SL https://github.com/docker/compose/releases/latest/download/docker-compose-linux-x86_64 -o /usr/local/bin/docker-compose
  sudo chmod +x /usr/local/bin/docker-compose
fi

echo "Bootstrap complete. Log out and back in to refresh docker group membership."
