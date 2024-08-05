



An implementation of Conduit-Realworld to demonstrate my primary code and technology competences
Dotnet Core API with security and devops practices
Technology: Dotnet Core 8 | JWT | MSSQL with SSDT | Docker with Compose | Postman tests
Security: three-level architecture (Perimeter, Business, Restricted) with PII encryption and auth token handling
Devops:  Docker Containers, Docker Compose with Networking, SSDT schema-as-code

# Conduit-Realworld API

# Setup
## Simple Deployment
TODO

## In Visual Studio
TODO

# Technology Stack
- Dotnet Core 8
- MSSQL with SSDT
- Docker with Compose

# Security
- Three-level architecture (Perimeter, Business, Restricted)
- JWE Tokens (not JWT)
- PII encryption

## Three Level Architecture
Standard security practice is to have multiple security zones. This application is set up to support that. 
Web API is in the Perimeter. It is the only part of the application that is exposed to the internet.
WebHost is in Business. It is the only part of the application that can access the database.
Database is in Restricted. It is a database. 

The same validation logic exists in both Web and WebHost. This allows WebHost to be used as an internal-facing API. Note that WebHost lacks swagger doc generation and access controls and is not ready for that use. 

## JWE Tokens
JWT is a standard way to handle authentication. However, JWT is not encrypted. It is signed. This means that the contents of the token are visible to anyone who has the token.
The content of the security token includes UserID. While this is not PII, it does give information about the system that we don't want to expose (eg how many users exist or how many users are created between two known timestamps).
JWE is a way to encrypt the token. This means that the contents of the token are not visible to anyone who has the token. 
Technically this means that this implementation may not be fully compatible with the Conduit-Realworld spec. However, it is more secure.

## PII Encryption
Email is PII and should be handled responsibly (and legally compliant). PII should not be stored in plaintext at rest. 
Since we need to get the email plaintext back for processing, it should be encrypted at rest. 
However we also need to be able to query the DB by email, including guaranteeing uniqueness (due to login needing email).
Encryption is nondeterministic. We can't search by encrypted value. This means we need to store email twice, in both encrypted and non-salted hash form. 
The email hashing algorithm is SHA-256. Encryption is AES. Standard stuff. 

Note that while emails are hashed with SHA-256, Passwords are hashed with Argon2id with the salt appended (ref OWASP below). This is because we need to be able to search by email, but we don't need to search by password.
Hashing with the same salt will produce the same hashtext, enabling password checking. Pretty standard stuff. 
https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html



- Postman tests


???

An implementation of the Conduit Realworld API in ASP.NET

Code demonstration for a job application.





TODO:
- Clean responses and error handling
- Tests?
- Docker
- OpenAPI 
- Readme 




Email is PII and should be handled responsibly (and legally compliant). PII should not be stored in plaintext at rest. 
Since we need to get the email plaintext back for processing, it should be encrypted at rest. 
However we also need to be able to query the DB by email, including guaranteeing uniqueness (due to login needing email).
Encryption is nondeterministic. We can't search by encrypted value. This means we need to store email twice, in both encrypted and non-salted hash form. 
The email hashing algorithm is SHA-256. Encryption is AES. Standard stuff. 


Passwords should not be stored in plaintext at rest. Standard practice (ref OWASP below) is to store using a cryptographic hash like Argon2id with the salt appended. 
Hashing with the same salt will produce the same hashtext, enabling password checking. Pretty standard stuff. 
https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html