# OAuth Curse Of Myrra

## Dependencies

- [Ueberauth-google](https://github.com/ueberauth/ueberauth_example) (depends on [Ueberauth](https://github.com/ueberauth/ueberauth))

# 0. Scope of First Iteration

Instead of allowing google login using oauth in smartphones, we’ll start with a web login and then transition that into client side login (VALIDATE WITH MANU).

```mermaid
sequenceDiagram
    Client(Web)->>+Server: login
    Server-->>+ProviderOAuth(Google/others): redirect
    ProviderOAuth(Google/others)-->>+Server: %{ email: "someemail", token: "sometoken" }
    Server-->>+Client(Web): %{ status: :ok, email: "someemail", token: "sometoken" }
```

# 1. Set the app in google developer console

1. https://www.wbotelhos.com/oauth-login-with-phoenix-and-ueberauth
2. https://developers.google.com/identity/protocols/oauth2

Follow the steps in 1. (**Credentials** section) You can keep 2. as referrence as well.

Note: You don’t need to set many fields, such as the home page. Fill the required ones and then Ok, Continue, Continue, Next …

As a result you should have your google oauth 2.0 client id.

# 2. Setup ueberauth-google

Follow https://github.com/ueberauth/ueberauth_google instructions

Notice that for each provider you need to add some different config as well.

# 3.  Create oauth controller

The oauth controller will use ueberauth to handle two step logins amongst different applications.

For each provider you’d want to define a `callback` function, which handles the provider's response containing different user data.

## 3.1 Google oauth response

Google’s response contains the following fields:

```elixir
%{
  uid: "USER_ID",
  provider: :google,
  strategy: Ueberauth.Strategy.Google,
  info: %Ueberauth.Auth.Info{
    name: nil,
    first_name: nil,
    last_name: nil,
    nickname: nil,
    email: "USER'S EMAIL",
    location: nil,
    description: nil,
    image: "URL",
    phone: nil,
    birthday: nil,
    urls: %{profile: nil, website: nil}
  },
  credentials: %Ueberauth.Auth.Credentials{
    token: "AUTH_TOKEN",
    refresh_token: nil,
    token_type: "Bearer",
    secret: nil,
    expires: true,
    expires_at: 1686933544,
    scopes: ["https://www.googleapis.com/auth/userinfo.email openid"],
    other: %{}
  },
  extra: %Ueberauth.Auth.Extra{
    raw_info: %{
      token: %OAuth2.AccessToken{
        access_token: "ya29.a0AWY7CkkCDCXGlWUu2R6b9DcAp3r7FFwZDx1rqFGB_SWp0qQ15L1ZbHNY88jLm3YBlUBrwa9eLA7fTCnvkEXTdG6mCxZJ1sKmlMZ25dorgLrLReJO4_eqVs-03mClqTRrJl858VYLL_xUUiADQF5kE5uEk4Y4aCgYKATMSARISFQG1tDrpOQTB56L8OijgiyBE2t_Lmg0163",
        refresh_token: nil,
        expires_at: 1686933544,
        token_type: "Bearer",
        other_params: %{
          "id_token" => "eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg1YmE5MzEzZmQ3YTdkNGFmYTg0ODg0YWJjYzg0MDMwMDQzNjMxODAiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIyNzg0NTg2MjM4NTAtdHEzNmswN29nbW92bG1yZWtmOGs4MXYwcHZmcDEwOHEuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIyNzg0NTg2MjM4NTAtdHEzNmswN29nbW92bG1yZWtmOGs4MXYwcHZmcDEwOHEuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDE5MzkxMzQyODkyMzc5MDQyOTMiLCJlbWFpbCI6InNhbmNoZXoubmljb2xhczk2QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoiX01MSGFyM290Y0ZtMnEwRE85Y2taUSIsImlhdCI6MTY4NjkyOTk0NSwiZXhwIjoxNjg2OTMzNTQ1fQ.Dfnhp35GS9gq7FGnOP2nsMH2W7XUdvBIJ6ifqFGcWGww65_bvYCd2cLWnjbhKik0OJBIy44nX5lv2ohbG2128Yy6XNTHTmFsU-wmxr3uw7xwhtAdCU6OcUb69k7tqPiX3z8NQ7u0MmQnAsD6bRsL9_6AWnK8dTQl433w-xZYa9ltWo8C8BVzG20Iq1z4Jb90eQR-4Mi3toVKIXavm4M_g-I2-uvYmTBFPrGCGsM9yZaR_pQpB9iAfdWJy-ElAJjCauIs_VbA1ymPS15GfUhLVuwEDaAKVgidlWL_thmQIgnO4YTI5HSUhJ9QKToqb92543O1Xk8aA_bEcSr14KHKiQ",
          "scope" => "https://www.googleapis.com/auth/userinfo.email openid"
        }
      },
      user: %{
        "email" => "USER's EMAIL",
        "email_verified" => true,
        "picture" => "URL",
        "sub" => "101939134289237904293"
      }
    }
  }
}
```

# 4. The User Schema

1. We reuse the email field (we’ll later see if we need to separate this)
2. We added some custom fields for the gmail token and the gmail secret
    1. Create an ecto migration with `mix ecto.gen.migration <migration_name>`
    2. For now we jsut store the google user id and the google email
3. add a custom changeset for your provider

# 5. OAuthController callback

The callback function will (if necessary) validate the response's  values for the user, and do an upsert operation for the corresponding user. It currently searches the user by email, but we could switch to an id.

# 6. Possible refactors

Now we use the previously existing `User` schema, which has the fields:

```elixir
field(:email, :string)
field(:password, :string, virtual: true, redact: true)
field(:hashed_password, :string, redact: true)
field(:confirmed_at, :naive_datetime)
field(:username, :string)
```

Since using providers remove the necessity of storing the password and confirmation, and the gmail token is not needed for regular users we could refactor the users into a structure like this:

```mermaid
erDiagram
    GoogleProviderUser }|..|{ UserData : has
    DiscordProviderUser }|..|{ UserData : has
    TwitterProviderUser }|..|{ UserData : has
    UserData {
        string username
        games[] played_games
        other_types other_things
    }
    GoogleProviderUser {
        string email
        string token
    }
```
