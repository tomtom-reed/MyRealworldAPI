

Email is PII and should be handled responsibly (and legally compliant). PII should not be stored in plaintext at rest. 
Since we need to get the email plaintext back for processing, it should be encrypted at rest. 
However we also need to be able to query the DB by email, including guaranteeing uniqueness (due to login needing email).
Encryption is nondeterministic. We can't search by encrypted value. This means we need to store email twice, in both encrypted and non-salted hash form. 
The email hashing algorithm is SHA-256. Encryption is AES. Standard stuff. 


Passwords should not be stored in plaintext at rest. Standard practice (ref OWASP below) is to store using a cryptographic hash like Argon2id with the salt appended. 
Hashing with the same salt will produce the same hashtext, enabling password checking. Pretty standard stuff. 
https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html