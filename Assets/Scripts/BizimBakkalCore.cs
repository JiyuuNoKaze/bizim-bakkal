Understood. Please provide the C# scripts, Supabase database schemas, and existing Row Level Security (RLS) policies for **Bizim Bakkal**.

Once shared, the audit will evaluate the following areas:

1. **Credential Safety & Network Layer:**
   * Verification of Supabase initialization (ensuring the `service_role` key is never bundled in client builds, only using the `anon` public key).
   * Secure transport (HTTPS/WSS) and token handling (secure storage of JWT session tokens).

2. **Supabase Row Level Security (RLS):**
   * Verification that RLS is explicitly enabled on all public tables (`ALTER TABLE ... ENABLE ROW LEVEL SECURITY;`).
   * Evaluation of policies to prevent unauthorized reads, updates, or spoofing of `auth.uid()`.
   * Game balance/economy integrity (ensuring critical transactions like currency updates or purchases cannot be arbitrary client writes).

3. **Local Data Security & Integrity:**
   * Cryptographic implementation for offline/cached save data (e.g., AES-GCM with platform-managed key storage such as DPAPI, Keychain, or Android Keystore, avoiding hardcoded static keys).
   * Anti-tamper checks (HMAC/signatures) to prevent save state manipulation.

4. **Resource Management & Headroom Compliance:**
   * Detection of C# event subscription leaks (unsubscribed delegates leading to GC retention).
   * Asset lifecycle tracking (proper `IDisposable` implementation, cleanup of textures/audio buffers on scene unload).
   * Minimal dependency footprint (relying on lightweight, native C# implementations rather than heavy third-party packages where possible).

Please paste the code and configurations to proceed.