# Rate limiting

To keep API Colombia fast and stable under heavy demand (**2M+ requests per month**), the busiest public endpoints apply a lightweight **per-IP rate limit**. The API stays open, free, and requires no authentication — the limit only trims abusive bursts.

## What is limited

A shared limit currently applies to the highest-traffic resource groups:

- **Holiday** — `/api/v1/Holiday/*`
- **City** — `/api/v1/City/*`
- **Department** — `/api/v1/Department/*`

All other endpoints are unaffected for now.

## The limit

| Property | Value |
| --- | --- |
| Scope | Per client IP address |
| Allowance | **60 requests per minute** |
| Algorithm | Sliding window (1-minute window, 10-second segments) |
| Shared budget | Holiday + City + Department count against the **same** per-IP budget |

Because the three groups share one budget, a client that mixes calls across them (e.g. 30 to City and 30 to Department in the same minute) consumes the full 60-request allowance.

## When you exceed it

Requests over the limit receive:

- **`429 Too Many Requests`**
- A **`Retry-After`** header (in seconds) telling you how long to wait
- A small JSON body:

```json
{ "message": "Rate limit exceeded. Please retry later." }
```

## Recommended: cache on your side

Most of this data changes rarely — holidays are deterministic, and cities/departments change infrequently. **You should not request it on every call.**

- Store the responses in a **cache or in-memory store** in your application and refresh them occasionally (e.g. once a day).
- The API already sends aggressive cache headers (responses are cacheable for up to **7 days**), so an HTTP cache or CDN in front of your app will also keep you well under the limit.
- Respect the `Retry-After` header with an exponential backoff if you ever hit a `429`.

Following these practices means a normal integration will effectively never reach the rate limit.

## Behind a proxy?

The limit is keyed on the client IP as seen by the API. Normal clients need to do nothing. If you operate a proxy or gateway in front of many users, be aware that all of them may appear under a single IP.
