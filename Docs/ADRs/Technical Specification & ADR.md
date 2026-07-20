# TECHNICAL SPECIFICATION: STATELESS JWT AUTHENTICATION & DATA ISOLATION

## 1. OBJECTIVE
The goal is to implement a secure, modern, and production-ready authentication system tailored for a decoupled Web API. By leveraging stateless JSON Web Tokens (JWT), we provide maximum flexibility for front-end applications (SPA/Mobile) while transforming the application into a multi-tenant environment where data is strictly isolated per user.

---

## 2. DATABASE SCHEMA DESIGN

To support data ownership, we will introduce a `Users` entity and establish relationships with existing tables.

### 2.1 The Users Table (`Users`)
* `Id` (int, Primary Key, Auto-Increment)
* `Email` (nvarchar(256), Unique Index, Required)
* `PasswordHash` (nvarchar(max), Required)
* `CreatedAt` / `UpdatedAt` (DateTime)

### 2.2 Expenses Table Modification
* Add `UserId` (int, Foreign Key pointing to `Users.Id`, Required).
* Every financial record must explicitly belong to the authenticated user who created it.
* A database index will be placed on `UserId` to optimize lookup queries.

### 2.3 Categories Table Modification
* Add `UserId` (int?, Foreign Key pointing to `Users.Id`, Nullable).
* **Hybrid Data Model:** 
  * If `UserId IS NULL`: The category is a global system default (e.g., "Food", "Utilities"), visible to everyone as read-only.
  * If `UserId` has a value: It is a custom category created by and private to that specific user.

---

## 3. SECURITY IMPLEMENTATION DETAILS

### 3.1 Cryptographic Password Hashing
Raw passwords will never touch the database. We will utilize **BCrypt.Net-Next** to securely salt and hash user passwords using a strong work factor before data persistence.

### 3.2 Stateless Token Flow
1. The front-end issues a `POST /api/auth/login`.
2. The server authenticates credentials and returns a signed JWT containing token metadata and essential user claims (`UserId`, `Email`).
3. The front-end stores this token securely and attaches it to the HTTP headers of all subsequent requests:
   `Authorization: Bearer <token>`
4. The server validates the cryptographic signature on every request using a secure symmetric key managed via `appsettings.json`.

---
---

# ARCHITECTURAL DECISION RECORD (ADR)

* **Title:** Choosing Stateless JWT Bearer Authentication over Stateful Session Cookies
* **Status:** Proposed / Approved
* **Context:** Our architecture is a pure, decoupled Web API designed to serve modern front-end clients. We require an authentication mechanism that handles cross-origin resource sharing (CORS) cleanly, minimizes server resource footprints, and aligns with professional REST API designs.

### Decision Rationale
We selected a custom, stateless JWT Bearer Token approach because:
1. **API-First Native Design:** Modern cross-origin front-ends frequently encounter cookie blockage, cross-site scripting (XSS), or CSRF complexities when dealing with pure APIs. JWTs pass cleanly via headers, completely bypassing cookie cross-domain policies.
2. **Horizontal Scalability:** The server retains zero session state in memory (RAM). This makes scaling the backend across multiple servers or cloud containers frictionless since any server can validate any incoming token instantly.
3. **Full Schema Control:** Building a tailored `Users` identity layout gives us 100% control over our database design and seamless alignment with our existing `ServiceResult<T>` and `ApiResponse<T>` code flow, avoiding the heavy, unneeded database tables bundled inside default frameworks like ASP.NET Core Identity.

### Consequences & Trade-offs
* **Mitigation of Revocation:** Stateless tokens cannot be instantly revoked without a centralized datastore (like Redis). To protect user security, we will issue tokens with short lifespans (e.g., 1 hour).