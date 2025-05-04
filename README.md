# JwtAuthenticationWithRefreshToken

## Project Summary
This project is a simple implementation of JWT authentication with refresh tokens using .Net 8. It includes a basic API for user registration, login, and token management. The project is designed to demonstrate the use of JWTs for secure authentication and authorization in a web application.

## Introduction
 - In a typical [JWT (JSON Web Token)](https://github.com/Benedict-Ik/JwtAuthentication) authentication setup, a user logs in and receives an access token—a digitally signed token that grants access to protected resources for a short duration (e.g., 15–30 minutes). 
 - However, once the access token expires, the user would normally need to log in again. 
 - To avoid this, a refresh token is introduced. A refresh token is a long-lived, secure token issued alongside the access token, and it can be stored safely (usually in an HttpOnly cookie or secure storage). 
 - When the access token expires, the client can send the refresh token to a dedicated endpoint (e.g., /token/refresh) to obtain a new access token without requiring the user to re-authenticate. 
 - This approach enhances both security (as access tokens are short-lived) and user experience (as re-login is minimized). 
 - The refresh token must be stored securely and can be revoked if compromised. Proper implementation also involves storing issued refresh tokens (or their hashes) server-side to detect misuse or reuse after logout or expiration.