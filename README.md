# Resilient Webhook Delivery Platform

[![CI](https://github.com/kdmbhushan/resilient-webhook-platform/actions/workflows/ci.yml/badge.svg)](https://github.com/kdmbhushan/resilient-webhook-platform/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple.svg)](https://dotnet.microsoft.com/)

> High-throughput asynchronous messaging architecture and webhook delivery platform built in C# .NET with Transactional Outbox pattern, Azure Service Bus / Redis backplanes, Polly v8 exponential backoff with full jitter, and HMAC-SHA256 signature verification.

📖 **Companion Deep Dive**: [Transactional Outbox & Resilient Messaging on Azure Service Bus](https://bhushankadam.dev/blog/azure-servicebus-outbox-pattern) on [bhushankadam.dev](https://bhushankadam.dev)

---

## 1. Problem Statement

In distributed microservices, delivering asynchronous webhooks to third-party customer endpoints with unpredictable latencies, slow responses, and transient failures causes outbound worker pool exhaustion, cascading failure across producer services, and security vulnerabilities without tamper-proof cryptographic signatures.

Simultaneously, dual-write operations between relational databases and message brokers inevitably cause split-brain data corruption when network partitions occur between the database commit and message dispatch.

---

## 2. Architecture

```mermaid
flowchart LR
    subgraph API["Ingestion & State"]
        A1["Business Mutation"]
        DB[("Relational DB + Outbox Table")]
        A1 -->|Atomic Transaction| DB
    end

    subgraph Outbox["Transactional Outbox Worker"]
        W1["Outbox Poller / CDC Worker"]
        DB -->|SKIP LOCKED Batch| W1
    end

    subgraph Dispatcher["Webhook Dispatcher Engine"]
        Q1["Partitioned Queue (Redis / AMQP)"]
        D1["Dispatcher Worker Pool"]
        P1["Polly v8 Resilience<br/>(Backoff + Jitter + Circuit Breaker)"]
        H1["HMAC-SHA256 Signer"]
        
        W1 --> Q1
        Q1 --> D1
        D1 --> P1
        D1 --> H1
    end

    subgraph External["External Consumers"]
        E1["Customer Webhook Endpoint"]
        DLQ["Dead-Letter Queue (DLQ)"]
        
        P1 -->|Signed HTTP POST| E1
        P1 -->|Max Retries Exceeded| DLQ
    end
```

---

## 3. Project Structure

```text
resilient-webhook-platform/
├── .github/workflows/
│   └── ci.yml                 # Automated build, test, and formatting verification
├── src/
│   ├── ResilientWebhook.Core/       # Domain entities: OutboxMessage, WebhookSubscription
│   ├── ResilientWebhook.Publisher/  # Transactional outbox background relay service
│   └── ResilientWebhook.Dispatcher/ # Webhook HTTP client, Polly policies, and HMAC-SHA256 signer
├── tests/
│   └── ResilientWebhook.Tests/      # Unit and verification test suites
└── README.md
```

---

## 4. Quickstart

### Build Solution
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

---

## 5. Security & Authenticity Disclosures

- **Zero Fabricated Claims**: This repository is a clean, reproducible reference implementation. It does not claim fictional enterprise client deployments.
- **Deterministic Validation**: All tests run locally without requiring third-party cloud subscriptions or paid external services.

---

## 6. License

Licensed under the [MIT License](LICENSE). Copyright &copy; 2026 Bhushan Kadam.
