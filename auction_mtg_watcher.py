#!/usr/bin/env python3
"""Poll auction.castlers.com for Magic: The Gathering items and email alerts."""

from __future__ import annotations

import argparse
import json
import os
import re
import smtplib
import ssl
import sys
from dataclasses import dataclass
from datetime import datetime, timezone
from email.message import EmailMessage
from pathlib import Path
from typing import Iterable
from urllib.parse import urljoin

import requests
from bs4 import BeautifulSoup

AUCTION_URL = "https://auction.castlers.com/"
STATE_FILE = Path(__file__).with_name(".auction_mtg_seen.json")
DEFAULT_RECIPIENTS = ["two.woods@gmail.com"]
USER_AGENT = "mtg-auction-watcher/1.1"


@dataclass(frozen=True)
class AuctionItem:
    item_id: str
    title: str
    current_bid: str
    time_left: str
    item_url: str


def fetch_html(url: str) -> str:
    response = requests.get(url, headers={"User-Agent": USER_AGENT}, timeout=30)
    response.raise_for_status()
    return response.text


def parse_items(html: str) -> list[AuctionItem]:
    soup = BeautifulSoup(html, "lxml")
    candidates: dict[str, AuctionItem] = {}

    for link in soup.find_all("a", href=True):
        text_blob = " ".join(link.stripped_strings)
        container = link.find_parent(["article", "li", "div", "tr"]) or link
        container_text = " ".join(container.stripped_strings)
        combined = f"{text_blob} {container_text}".lower()

        if "magic" not in combined or "gathering" not in combined:
            continue

        title = next((t.strip() for t in [link.get("title"), text_blob] if t and t.strip()), "Magic: The Gathering Item")

        bid_match = re.search(r"(?:current\s*bid|bid)\s*[:\-]?\s*(\$\s?[\d,]+(?:\.\d{2})?)", container_text, flags=re.I)
        time_match = re.search(r"(?:time\s*left|ends?\s*in|closing\s*in)\s*[:\-]?\s*([\w\s,:-]{2,40})", container_text, flags=re.I)

        item_url = urljoin(AUCTION_URL, link["href"])
        item_id = re.sub(r"\W+", "-", item_url.lower()).strip("-")

        candidates[item_id] = AuctionItem(
            item_id=item_id,
            title=title,
            current_bid=bid_match.group(1) if bid_match else "Unknown",
            time_left=time_match.group(1).strip() if time_match else "Unknown",
            item_url=item_url,
        )

    return list(candidates.values())


def load_seen(path: Path) -> set[str]:
    if not path.exists():
        return set()
    try:
        data = json.loads(path.read_text(encoding="utf-8"))
    except json.JSONDecodeError:
        return set()
    return {str(x) for x in data} if isinstance(data, list) else set()


def save_seen(path: Path, seen: Iterable[str]) -> None:
    path.write_text(json.dumps(sorted(set(seen)), indent=2), encoding="utf-8")


def send_email(items: list[AuctionItem], recipients: list[str]) -> None:
    host = os.environ.get("SMTP_HOST")
    user = os.environ.get("SMTP_USER")
    password = os.environ.get("SMTP_PASS")
    port = int(os.environ.get("SMTP_PORT", "587"))
    sender = os.environ.get("SMTP_FROM", user)

    if not all([host, user, password, sender]):
        raise RuntimeError("Missing SMTP config. Set SMTP_HOST, SMTP_USER, SMTP_PASS, and SMTP_FROM.")

    now = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M UTC")
    lines = [f"New Magic: The Gathering auction item(s) found at {now}:", ""]
    for item in items:
        lines.extend([
            f"Title: {item.title}",
            f"Current bid: {item.current_bid}",
            f"Time left: {item.time_left}",
            f"URL: {item.item_url}",
            "",
        ])

    msg = EmailMessage()
    msg["Subject"] = f"[{len(items)}] New MTG auction item(s) on castlers.com"
    msg["From"] = sender
    msg["To"] = ", ".join(recipients)
    msg.set_content("\n".join(lines))

    context = ssl.create_default_context()
    with smtplib.SMTP(host, port, timeout=30) as smtp:
        smtp.starttls(context=context)
        smtp.login(user, password)
        smtp.send_message(msg)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--state", default=str(STATE_FILE), help="Path to seen-items state file")
    parser.add_argument("--dry-run", action="store_true", help="Print findings and skip email")
    args = parser.parse_args()

    try:
        html = fetch_html(AUCTION_URL)
    except requests.RequestException as exc:
        print(f"Failed to fetch {AUCTION_URL}: {exc}", file=sys.stderr)
        return 2

    items = parse_items(html)
    if not items:
        print("No Magic: The Gathering items found.")
        return 0

    seen = load_seen(Path(args.state))
    new_items = [item for item in items if item.item_id not in seen]

    if not new_items:
        print(f"Found {len(items)} item(s), but all were already reported.")
        return 0

    recipients = DEFAULT_RECIPIENTS.copy()
    extra = os.environ.get("ALERT_RECIPIENTS")
    if extra:
        recipients.extend([x.strip() for x in extra.split(",") if x.strip()])

    if args.dry_run:
        print(f"Would notify {', '.join(recipients)} about {len(new_items)} new item(s):")
        for item in new_items:
            print(f"- {item.title} | bid={item.current_bid} | time_left={item.time_left}")
    else:
        send_email(new_items, recipients)
        print(f"Sent notification for {len(new_items)} new item(s) to {', '.join(recipients)}")

    seen.update(item.item_id for item in items)
    save_seen(Path(args.state), seen)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
