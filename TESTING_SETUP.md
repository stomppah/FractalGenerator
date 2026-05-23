# EC2 + Windows 11 Testing Setup

## 1) Local build/test on Windows 11
1. Install **Docker Desktop**, **Git**, and **Python 3.12+**.
2. Clone this repo.
3. Copy `.env.test.example` to `.env.test` and fill in SMTP values.
4. Run local dry run:
   ```bash
   python auction_mtg_watcher.py --dry-run
   ```
5. Run containerized test:
   ```bash
   docker compose -f docker-compose.test.yml --env-file .env.test up --build --abort-on-container-exit
   ```

## 2) VS Code / Visual Studio workflow
- Use VS Code with recommended extensions in `.vscode/extensions.json`.
- Use tasks in `.vscode/tasks.json`:
  - **Watcher: Dry Run**
  - **Docker Compose: Test Run**

> Visual Studio 2022 can also run Docker Compose projects, but this Python-first workflow is fastest in VS Code.

## 3) EC2 test environment deployment
### On a fresh Amazon Linux 2023 EC2 instance:
```bash
chmod +x ops/ec2-test/bootstrap-amazon-linux.sh
./ops/ec2-test/bootstrap-amazon-linux.sh
```

### Deploy test run:
```bash
git clone <your-repo-url> FractalGenerator
cd FractalGenerator
cp .env.test.example .env.test
# Edit .env.test with SMTP credentials
./ops/ec2-test/deploy-test.sh
```

## 4) Cron deployment on EC2
After you validate in test mode:
```bash
crontab auction_mtg_watcher.cron
```

Then update real SMTP settings in the cron line or source them from a secure env file.
