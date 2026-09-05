# Discord Event Time

Discord Event Time now includes a Blazor WebAssembly web app in `Discord.EventTime.Web` so people can use it from a browser instead of running the desktop `.exe`.

## Share it as a website

This app can be hosted on **GitHub Pages**. That is enough for this project because the web app is static and does not need a backend server.

Expected public URL after GitHub Pages is enabled:

`https://targanon.github.io/Discord-Event-time/`

## What is already set up

The repo now includes:

- a GitHub Actions workflow at `.github/workflows/deploy-web-to-github-pages.yml`
- a publish preparation script at `scripts/Prepare-GitHubPages.ps1`
- GitHub Pages fixes for the Blazor base path, PWA manifest, and SPA fallback

## What you need to do on GitHub

1. Push this branch to GitHub.
2. Open the repository on GitHub.
3. Go to **Settings > Pages**.
4. Under **Build and deployment**, set **Source** to **GitHub Actions**.
5. Open the **Actions** tab and run or wait for **Deploy Discord Event Time Web to GitHub Pages**.
6. After the workflow finishes, open:
   - `https://targanon.github.io/Discord-Event-time/`

## Local testing

Run the `Discord.EventTime.Web` project from Visual Studio to test the web app locally before pushing.

## Notes

- GitHub Pages hosts the site over HTTPS.
- Users only need the link.
- They do not need to install the desktop app.
- The PWA install option depends on the browser, but the site itself will still work as a normal web page.